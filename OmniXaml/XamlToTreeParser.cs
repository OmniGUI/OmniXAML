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
        private readonly IAssignmentExtractor assignmentExtractor;
        private readonly DirectiveExtractor directiveExtractor;
        private readonly IMetadataProvider metadataProvider;
        private readonly IResolver resolver;

        public XamlToTreeParser(IMetadataProvider metadataProvider, IEnumerable<IInlineParser> inlineParsers, IResolver resolver)
        {
            this.metadataProvider = metadataProvider;
            this.resolver = resolver;
            Func<XElement, IPrefixAnnotator, ConstructionNode> func = ProcessNode;
            assignmentExtractor = new AssignmentExtractor(metadataProvider, inlineParsers, resolver, func);
            directiveExtractor = new DirectiveExtractor();
        }

        public ParseResult Parse(string xml, IPrefixAnnotator prefixAnnotator)
        {
            var xDocument = XDocument.Load(new StringReader(xml));
            var node = xDocument.FirstNode;
            return new ParseResult
            {
                Root = ProcessNode((XElement)node, prefixAnnotator),
                PrefixAnnotator = prefixAnnotator,
            };
        }

        private ConstructionNode ProcessNode(XElement node, IPrefixAnnotator annotator)
        {
            var type = resolver.LocateType(node.Name);
            var rawAssigments = assignmentExtractor.GetAssignments(type, node, annotator).ToList();
            var directives = directiveExtractor.GetDirectives(node).ToList();

            var attributeBasedInstanceProperties = CombineDirectivesAndAssigments(type, directives, rawAssigments);

            var children = GetChildren(type, node, annotator);

            var ctorArgs = GetCtorArgs(node, type);

            var constructionNode = new ConstructionNode(type)
            {
                Name = attributeBasedInstanceProperties.Name,
                Key = attributeBasedInstanceProperties.Key,
                Assignments = attributeBasedInstanceProperties.Assignments,
                InjectableArguments = ctorArgs,
                Children = children,
                InstantiateAs = attributeBasedInstanceProperties.InstantiateAs,
            };

            AnnotatePrefixes(node, annotator, constructionNode);

            return constructionNode;
        }

        private static void AnnotatePrefixes(XElement node, IPrefixAnnotator annotator, ConstructionNode constructionNode)
        {
            var prefixes = GetPrefixes(node).ToList();
            if (prefixes.Any())
            {
                annotator.Annotate(constructionNode, prefixes);
            }
        }

        private static IEnumerable<PrefixDeclaration> GetPrefixes(XElement node)
        {
            return node
                .Attributes()
                .Where(attribute => attribute.IsNamespaceDeclaration)
                .Select(
                    attribute =>
                    {
                        var isEmpty = attribute.Name.NamespaceName == "";
                        return isEmpty
                            ? new PrefixDeclaration(string.Empty, attribute.Value)
                            : new PrefixDeclaration(attribute.Name.LocalName, attribute.Value);
                    });
        }

        private IEnumerable<ConstructionNode> GetChildren(Type type, XElement node, IPrefixAnnotator annotator)
        {
            var hasContentProperty = metadataProvider.Get(type).ContentProperty != null;

            if (!hasContentProperty)
            {
                var childNodes = node.Nodes().OfType<XElement>().Where(element => !IsProperty(element));
                return childNodes.Select(e => ProcessNode(e, annotator));
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
            var classDirectiveValue = directives.FirstOrDefault(directive => directive.Name == "Class")?.Value;
            Type instantiateAs = null;

            if (classDirectiveValue != null)
            {
                instantiateAs = resolver.LocateTypeForClassDirective(type, classDirectiveValue);
            }

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
                InstantiateAs = instantiateAs,
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