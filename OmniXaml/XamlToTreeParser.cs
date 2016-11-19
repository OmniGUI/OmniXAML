namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Metadata;

    public class XamlToTreeParser : IXamlToTreeParser
    {
        private readonly IAssignmentExtractor assignmentExtractor;
        private readonly DirectiveExtractor directiveExtractor;
        private readonly IMetadataProvider metadataProvider;
        private readonly IResolver resolver;

        public XamlToTreeParser(IMetadataProvider metadataProvider, IEnumerable<IInlineParser> inlineParsers, IResolver resolver)
        {
            this.metadataProvider = metadataProvider;
            this.resolver = resolver;
            assignmentExtractor = new AssignmentExtractor(metadataProvider, inlineParsers, resolver, ProcessNode);
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
            var type = resolver.LocateType(node.Name);
            var rawAssigments = assignmentExtractor.GetAssignments(type, node).ToList();
            var directives = directiveExtractor.GetDirectives(node).ToList();

            var attributeBasedInstanceProperties = CombineDirectivesAndAssigments(type, directives, rawAssigments);

            var children = GetChildren(type, node);

            var ctorArgs = GetCtorArgs(node, type);

            return new ConstructionNode(type)
            {
                Name = attributeBasedInstanceProperties.Name,
                Key = attributeBasedInstanceProperties.Key,
                Assignments = attributeBasedInstanceProperties.Assignments,
                InjectableArguments = ctorArgs,                
                Children = children,
            };
        }

        private IEnumerable<ConstructionNode> GetChildren(Type type, XElement node)
        {
            var hasContentProperty = metadataProvider.Get(type).ContentProperty != null;

            if (!hasContentProperty)
            {
                var childNodes = node.Nodes().OfType<XElement>().Where(element => !IsProperty(element));
                return childNodes.Select(ProcessNode);
            }

            return new List<ConstructionNode>();
        }

        private static bool IsProperty(XElement node)
        {
            return node.Name.LocalName.Contains(".");
        }

        private AttributeBasedInstanceProperties CombineDirectivesAndAssigments(Type type, IList<Directive> directives, IList<MemberAssignment> assignments)
        {
            var allAssignments = assignments;

            var nameDirectiveValue = directives.FirstOrDefault(directive => directive.Name == "Name")?.Value;
            var key = directives.FirstOrDefault(directive => directive.Name == "Key")?.Value;

            var namePropertyName = metadataProvider.Get(type).RuntimePropertyName;
            string name = null;
            IEnumerable<MemberAssignment> finalAssignments = allAssignments;

            var nameAssignment = allAssignments.FirstOrDefault(assignment => assignment.Member.MemberName == namePropertyName);
            if (namePropertyName != null && nameAssignment != null)
            {
                name = nameAssignment.SourceValue;
                finalAssignments = allAssignments;
            }
            else if (nameDirectiveValue != null && namePropertyName != null)
            {
                name = nameDirectiveValue;
                var nameAssigment = new MemberAssignment { SourceValue = nameDirectiveValue, Member = Member.FromStandard(type, namePropertyName) };
                finalAssignments = allAssignments.Concat(new[] { nameAssigment });
            }

            return new AttributeBasedInstanceProperties
            {
                Name = name,
                Key = key,
                Assignments = finalAssignments,                
            };
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
}