namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Glass.Core;
    using Metadata;
    using TypeLocation;

    public class AssignmentExtractor
    {
        private const string SpecialNamespace = "special";
        private readonly IEnumerable<IInlineParser> inlineParsers;
        private readonly IMetadataProvider metadataProvider;
        private readonly Func<XElement, ConstructionNode> parser;
        private readonly ITypeDirectory typeDirectory;

        public AssignmentExtractor(IMetadataProvider metadataProvider,
            ITypeDirectory typeDirectory,
            IEnumerable<IInlineParser> inlineParsers,
            Func<XElement, ConstructionNode> parser)
        {
            this.metadataProvider = metadataProvider;
            this.typeDirectory = typeDirectory;
            this.inlineParsers = inlineParsers;
            this.parser = parser;
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
            var prop = node.Name;
            var name = prop.LocalName.SkipWhile(c => c != '.').Skip(1);
            var propertyName = new string(name.ToArray());

            var member = ResolveProperty(type, node);

            var nodeFirstNode = node.FirstNode;
            if ((nodeFirstNode != null) && ((nodeFirstNode.NodeType == XmlNodeType.Text) || (nodeFirstNode.NodeType == XmlNodeType.CDATA)))
            {
                var value = ((XText) nodeFirstNode).Value;

                return new MemberAssignment {Member = member, SourceValue = value};
            }
            var children = node.Elements().Select(parser);
            return new MemberAssignment {Member = member, Children = children};
        }

        private Member ResolveProperty(Type type, XElement element)
        {
            var nameLocalName = element.Name.LocalName;

            var dot = nameLocalName.IndexOf('.');
            var propertyName = nameLocalName.Skip(dot + 1).AsString();
            
            var ownerType = LocateType(element.Name);

            if (ownerType == type)
                return Member.FromStandard(ownerType, propertyName);
            return Member.FromAttached(ownerType, propertyName);
        }

        public IEnumerable<MemberAssignment> GetAssignmentsFromAttributes(Type type, XElement node)
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
            var property = ResolveProperty(type, attribute);

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

        private Member ResolveProperty(Type type, XAttribute attribute)
        {
            var nameLocalName = attribute.Name.LocalName;
            if (nameLocalName.Contains('.'))
            {
                var dot = nameLocalName.IndexOf('.');
                var ownerName = nameLocalName.Take(dot).AsString();
                var propertyName = nameLocalName.Skip(dot + 1).AsString();

                var xname = attribute.Name.NamespaceName == string.Empty
                    ? XName.Get(ownerName, attribute.Parent.Name.NamespaceName)
                    : XName.Get(ownerName, attribute.Name.NamespaceName);
                var ownerType = LocateType(xname);
                return Member.FromAttached(ownerType, propertyName);
            }
            return Member.FromStandard(type, nameLocalName);
        }

        private Type LocateType(XName typeName)
        {
            return typeDirectory.GetTypeByFullAddress(
                new Address
                {
                    Namespace = typeName.NamespaceName,
                    TypeName = typeName.LocalName
                });
        }
    }
}