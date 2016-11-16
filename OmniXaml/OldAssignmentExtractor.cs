namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Metadata;

    public class OldAssignmentExtractor : IAssignmentExtractor
    {
        private const string SpecialNamespace = "special";
        private readonly IEnumerable<IInlineParser> inlineParsers;
        private readonly IMetadataProvider metadataProvider;
        private readonly Func<XElement, ConstructionNode> parser;
        private readonly Resolver resolver;

        public OldAssignmentExtractor(IMetadataProvider metadataProvider,
            Resolver resolver,
            IEnumerable<IInlineParser> inlineParsers,
            Func<XElement, ConstructionNode> parser)
        {
            this.metadataProvider = metadataProvider;
            this.inlineParsers = inlineParsers;
            this.parser = parser;
            this.resolver = resolver;
        }

        public IEnumerable<MemberAssignment> GetAssignments(Type type, XElement node)
        {
            IEnumerable<XElement> innerElementsThatCanBeAssigments;
            if (metadataProvider.Get(type).ContentProperty == null)
                innerElementsThatCanBeAssigments = node.Nodes().OfType<XElement>().Where(IsProperty);
            else
                innerElementsThatCanBeAssigments = node.Nodes().OfType<XElement>();

            return GetAssignmentsFromAttributes(type, node)
                .Concat(GetAssignmentsFromContent(type, node))
                .Concat(GetAssignmentsFromInnerElements(type, innerElementsThatCanBeAssigments));
        }

        private IEnumerable<MemberAssignment> GetAssignmentsFromContent(Type type, XContainer node)
        {
            var nodeFirstNode = node.FirstNode;
            if ((nodeFirstNode != null) && (nodeFirstNode.NodeType == XmlNodeType.Text) && (metadataProvider.Get(type).ContentProperty != null))
            {
                var directContent = ((XText) nodeFirstNode).Value;
                var contentProperty = metadataProvider.Get(type).ContentProperty;
                var property = Member.FromStandard(type, contentProperty);
                yield return new MemberAssignment {Member = property, SourceValue = directContent};
            }
        }

        private IEnumerable<MemberAssignment> GetAssignmentsFromInnerElements(Type type, IEnumerable<XElement> nodes)
        {
            return nodes.Select(
                node => IsProperty(node)
                    ? ProcessExplicityProperty(type, node)
                    : ProcessImplicityProperty(type, node));
        }

        private MemberAssignment ProcessImplicityProperty(Type type, XElement node)
        {
            var constructionNode = parser(node);
            var contentProperty = metadataProvider.Get(type).ContentProperty;
            if (contentProperty == null)
                throw new XamlParserException($"Cannot assign node. The content property of the type {type} cannot be found");

            return new MemberAssignment {Member = Member.FromStandard(type, contentProperty), Children = new[] {constructionNode}};
        }

        private static bool IsProperty(XElement node)
        {
            return node.Name.LocalName.Contains(".");
        }

        private MemberAssignment ProcessExplicityProperty(Type type, XElement node)
        {
            var member = resolver.ResolveProperty(type, node);

            var nodeFirstNode = node.FirstNode;
            if ((nodeFirstNode != null) && ((nodeFirstNode.NodeType == XmlNodeType.Text) || (nodeFirstNode.NodeType == XmlNodeType.CDATA)))
            {
                var value = ((XText) nodeFirstNode).Value;
                return new MemberAssignment {Member = member, SourceValue = value};
            }

            var children = node.Elements().Select(parser);
            return new MemberAssignment {Member = member, Children = children};
        }

        private IEnumerable<MemberAssignment> GetAssignmentsFromAttributes(Type type, XElement node)
        {
            return node
                .Attributes()
                .Where(IsAssignment)
                .Select(attribute => ToAssignment(type, attribute));
        }

        private static bool IsAssignment(XAttribute attribute)
        {
            return !(attribute.IsNamespaceDeclaration || (attribute.Name.Namespace == SpecialNamespace));
        }

        private MemberAssignment ToAssignment(Type type, XAttribute attribute)
        {
            var value = attribute.Value;
            var property = resolver.ResolveProperty(type, attribute);

            var inlineParser = inlineParsers.FirstOrDefault(p => p.CanParse(value));
            if (inlineParser != null)
            {
                var constructionNode = inlineParser.Parse(value);
                return new MemberAssignment {Member = property, Children = new[] {constructionNode}};
            }

            var assignment = new MemberAssignment
            {
                Member = property,
                SourceValue = value
            };

            return assignment;
        }
    }

    public interface IAssignmentExtractor
    {
        IEnumerable<MemberAssignment> GetAssignments(Type type, XElement element);
    }
}