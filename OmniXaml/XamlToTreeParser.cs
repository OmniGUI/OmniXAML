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
        private readonly DirectiveExtractor directiveExtractor;
        private readonly IMetadataProvider metadataProvider;
        private readonly ITypeDirectory typeDirectory;

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
            var rawAssigments = assignmentExtractor.GetAssignments(type, node);
            var directives = directiveExtractor.GetDirectives(node);

            var instanceProperties = CombineDirectivesAndAssigments(type, directives, rawAssigments);

            var ctorArgs = GetCtorArgs(node, type);

            return new ConstructionNode(type) { Assignments = instanceProperties.Assignments, InjectableArguments = ctorArgs, Name = instanceProperties.Name };
        }

        private InstanceProperties CombineDirectivesAndAssigments(Type type, IEnumerable<Directive> directives, IEnumerable<PropertyAssignment> assignments)
        {
            var allAssignments = assignments.ToList();

            var nameDirectiveValue = directives.FirstOrDefault(directive => directive.Name == "Name")?.Value;
            var namePropertyName = metadataProvider.Get(type).RuntimePropertyName;
            string name = null;
            IEnumerable<PropertyAssignment> finalAssignments = allAssignments;

            var nameAssignment = allAssignments.FirstOrDefault(assignment => assignment.Property.PropertyName == namePropertyName);
            if (namePropertyName != null && nameAssignment != null)
            {
                name = nameAssignment.SourceValue;
                finalAssignments = allAssignments;
            }
            else if (nameDirectiveValue != null && namePropertyName != null)
            {
                name = nameDirectiveValue;
                var nameAssigment = new PropertyAssignment { SourceValue = nameDirectiveValue, Property = Property.RegularProperty(type, namePropertyName) };
                finalAssignments = allAssignments.Concat(new[] { nameAssigment });
            }

            return new InstanceProperties
            {
                Name = name,
                Assignments = finalAssignments,
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

    internal class InstanceProperties
    {
        public string Name { get; set; }
        public IEnumerable<PropertyAssignment> Assignments { get; set; }
    }
}