namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Metadata;
    using Zafiro.Core;

    public class AssignmentExtractor : IAssignmentExtractor
    {
        private const string SpecialNamespace = "special";
        private readonly Func<XElement, IPrefixAnnotator, ConstructionNode> createFunc;

        private readonly IEnumerable<IInlineParser> inlineParsers;
        private readonly IMetadataProvider metadataProvider;
        private readonly IXmlTypeResolver xmlTypeResolver;

        public AssignmentExtractor(IMetadataProvider metadataProvider,
            IEnumerable<IInlineParser> inlineParsers,
            IXmlTypeResolver xmlTypeResolver,
            Func<XElement, IPrefixAnnotator, ConstructionNode> createFunc)
        {
            this.metadataProvider = metadataProvider;
            this.inlineParsers = inlineParsers;
            this.xmlTypeResolver = xmlTypeResolver;
            this.createFunc = createFunc;
        }

        public IEnumerable<MemberAssignment> GetAssignments(Type owner, XElement element, IPrefixAnnotator annotator)
        {
            EnsureValidAssignments(element);

            var fromAttributes = FromAttributes(owner, element);
            var fromPropertyElements = FromPropertyElements(owner, element, annotator);
            var fromContentProperty = FromContentProperty(owner, element, annotator);

            return fromAttributes.Concat(fromContentProperty).Concat(fromPropertyElements);
        }

        private static void EnsureValidAssignments(XContainer element)
        {
            var numberOfChanges = element
                .Elements()
                .Select(IsProperty)
                .DistinctUntilChanged()
                .Count();                

            if (numberOfChanges > 2)
            {
                throw new ParseException("Property elements cannot be in the middle of an element's content. They must be before or after the content.");
            }
        }

        private IEnumerable<MemberAssignment> FromContentProperty(Type type, XElement element, IPrefixAnnotator annotator)
        {
            var contentProperty = metadataProvider.Get(type).ContentProperty;

            if (contentProperty == null)
            {
                yield break;
            }

            var propertyElements = GetPropertyElements(element);

            var propertyElementsList = propertyElements.ToList();

            if (propertyElementsList.Any())
            {
                yield return new MemberAssignment
                {
                    Member = Member.FromStandard(type, contentProperty),
                    Values = propertyElementsList.Select(e => createFunc(e, annotator)).ToList()
                };
            }
            else
            {
                var elementFirstNode = element.FirstNode;

                if (elementFirstNode != null && IsText(elementFirstNode))
                {
                    yield return new MemberAssignment
                    {
                        Member = Member.FromStandard(type, contentProperty),
                        SourceValue = ((XText)elementFirstNode).Value,
                    };
                }
            }
        }

        private static IEnumerable<XElement> GetPropertyElements(XContainer parent)
        {
            return from e in parent.Elements() where !IsProperty(e) select e;
        }

        private IEnumerable<MemberAssignment> FromPropertyElements(Type type, XElement element, IPrefixAnnotator annotator)
        {
            var propertyElements = element.Elements().Where(IsProperty);

            foreach (var propertyElement in propertyElements)
            {
                yield return FromPropertyElement(type, propertyElement, annotator);
            }
        }

        private MemberAssignment FromPropertyElement(Type type, XElement propertyElement, IPrefixAnnotator annotator)
        {
            var member = xmlTypeResolver.ResolveProperty(type, propertyElement);
            var children = propertyElement.Elements().Select(e => createFunc(e, annotator));

            var directValue = GetDirectValue(propertyElement);

            if (directValue != null && children.Any())
            {
                throw new Exception("Cannot have a direct value and child nodes at the same time");
            }

            return new MemberAssignment {Member = member, Values = children, SourceValue = directValue};
        }

        private static string GetDirectValue(XContainer node)
        {
            var nodeFirstNode = node.FirstNode;
            if (nodeFirstNode != null && IsText(nodeFirstNode))
            {
                return ((XText) nodeFirstNode).Value;
            }

            return null;
        }

        private static bool IsText(XObject nodeFirstNode)
        {
            return nodeFirstNode.NodeType == XmlNodeType.Text || nodeFirstNode.NodeType == XmlNodeType.CDATA;
        }

        private IEnumerable<MemberAssignment> FromAttributes(Type type, XElement element)
        {
            return element
                .Attributes()
                .Where(IsAssignment)
                .Select(attribute => ToAssignment(type, attribute));
        }

        private static bool IsAssignment(XAttribute attribute)
        {
            return !(attribute.IsNamespaceDeclaration || attribute.Name.Namespace == SpecialNamespace);
        }

        private MemberAssignment ToAssignment(Type type, XAttribute attribute)
        {
            var value = attribute.Value;
            var property = xmlTypeResolver.ResolveProperty(type, attribute);

            var inlineParser = inlineParsers.FirstOrDefault(p => p.CanParse(value));
            if (inlineParser != null)
            {
                var constructionNode = inlineParser.Parse(value, prefix => GetNamespaceFromPrefix(attribute, prefix));
                return new MemberAssignment {Member = property, Values = new[] {constructionNode}};
            }

            var assignment = new MemberAssignment
            {
                Member = property,
                SourceValue = value,
            };

            return assignment;
        }

        private static string GetNamespaceFromPrefix(XObject attribute, string prefix)
        {
            var attributeParent = attribute.Parent;

            if (prefix == string.Empty)
            {
                var defaultNamespace = attributeParent.GetDefaultNamespace();
                return defaultNamespace.NamespaceName;
            }

            return attributeParent.GetNamespaceOfPrefix(prefix).NamespaceName;
        }

        private static bool IsProperty(XElement node)
        {
            return node.Name.LocalName.Contains(".");
        }
    }

    
}