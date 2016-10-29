namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Metadata;
    using TypeLocation;

    public class XamlToTreeParser : IXamlToTreeParser
    {
        private readonly AssignmentExtractor assignmentExtractor;
        private readonly IMetadataProvider metadataProvider;
        private readonly ITypeDirectory typeDirectory;
        private readonly DirectiveExtractor directiveExtractor;

        public XamlToTreeParser(ITypeDirectory typeDirectory, IMetadataProvider metadataProvider, IEnumerable<IInlineParser> inlineParsers)
        {
            this.typeDirectory = typeDirectory;
            this.metadataProvider = metadataProvider;
            assignmentExtractor = new AssignmentExtractor(metadataProvider, typeDirectory, inlineParsers, ProcessNode);
            directiveExtractor = new DirectiveExtractor();
        }


        public ConstructionNode Parse(string xml)
        {
            var xm = XDocument.Load(new StringReader(xml));
            var node = xm.FirstNode;
            return ProcessNode((XElement)node);
        }

        private ConstructionNode ProcessNode(XElement node)
        {
            var type = LocateType(node.Name);
            var allAssignments = assignmentExtractor.GetAssignments(type, node);
            var directives = directiveExtractor.GetDirectives(node);

            var nameAssignment = GetNameProperties(type, directives, allAssignments);

            if (nameAssignment.NameAssignment != null)
            {
                allAssignments = allAssignments.Concat(new[] { nameAssignment.NameAssignment });
            }

            var ctorArgs = GetCtorArgs(node, type);

            return new ConstructionNode(type) { Assignments = allAssignments, InjectableArguments = ctorArgs, Name = nameAssignment.Name };
        }

        private NameProperties GetNameProperties(Type type, IEnumerable<Directive> directives, IEnumerable<PropertyAssignment> allAssignments)
        {
            var name = directives.FirstOrDefault(directive => directive.Name == "Name")?.Value;
            var namePropertyName = metadataProvider.Get(type).RuntimePropertyName;

            var firstOrDefault = allAssignments.FirstOrDefault(assignment => assignment.Property.PropertyName == namePropertyName);
            if (namePropertyName != null && firstOrDefault != null)
            {
                return new NameProperties()
                {
                    Name = firstOrDefault.SourceValue,
                };
            }

            PropertyAssignment propertyAssignment = null;
            if (name != null && namePropertyName != null)
            {
                propertyAssignment = new PropertyAssignment { SourceValue = name, Property = Property.RegularProperty(type, namePropertyName) };
            }

            return new NameProperties
            {
                Name = name,
                NameAssignment = propertyAssignment,
            };
        }



        private Type LocateType(XName typeName)
        {
            return typeDirectory.GetTypeByFullAddres(
                new Address
                {
                    Namespace = typeName.NamespaceName,
                    TypeName = typeName.LocalName
                });
        }

        private IEnumerable<string> GetCtorArgs(XContainer node, Type type)
        {
            var ctorArgs = new List<string>();

            var nodeFirstNode = node.FirstNode;
            if ((nodeFirstNode != null) && (nodeFirstNode.NodeType == XmlNodeType.Text))
            {
                var directContent = ((XText)nodeFirstNode).Value;
                var contentProperty = metadataProvider.Get(type).ContentProperty;
                if (contentProperty == null)
                    ctorArgs.Add(directContent);
            }
            return ctorArgs;
        }
    }

    internal class NameProperties
    {
        public string Name { get; set; }
        public PropertyAssignment NameAssignment { get; set; }
    }
}