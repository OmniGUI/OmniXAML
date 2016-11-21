namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Metadata;

    public class AssignmentExtractor : IAssignmentExtractor
    {
        private readonly IMetadataProvider metadataProvider;

        private readonly IEnumerable<IInlineParser> inlineParsers;
        private readonly IResolver resolver;
        private readonly Func<XElement, IPrefixAnnotator, ConstructionNode> createFunc;

        private const string SpecialNamespace = "special";

        public AssignmentExtractor(IMetadataProvider metadataProvider, IEnumerable<IInlineParser> inlineParsers, IResolver resolver, Func<XElement, IPrefixAnnotator, ConstructionNode> createFunc)
        {
            this.metadataProvider = metadataProvider;
            this.inlineParsers = inlineParsers;
            this.resolver = resolver;
            this.createFunc = createFunc;
        }

        public IEnumerable<MemberAssignment> GetAssignments(Type type, XElement element, IPrefixAnnotator annotator)
        {
            EnsureValidAssignments(element);

            var fromAttributes = FromAttributes(type, element);
            var fromPropertyElements = FromPropertyElements(type, element, annotator);
            var fromContentProperty = FromContentProperty(type, element, annotator);

            return fromAttributes.Concat(fromContentProperty).Concat(fromPropertyElements);
        }

        private static void EnsureValidAssignments(XContainer element)
        {
            var numberOfChanges = element
                .Elements()                
                .ToObservable()
                .Select(IsProperty)
                .DistinctUntilChanged()
                .Count()
                .ToEnumerable()
                .First();

            if (numberOfChanges > 2)
            {
                throw new ParseException("Property elements cannot be in the middle of an element's content. They must be before or after the content.");
            }
        }

        private IEnumerable<MemberAssignment> FromContentProperty(Type type, XElement element, IPrefixAnnotator annotator)
        {
            var propertyElements = element.Elements().Where(xElement => !IsProperty(xElement)).ToList();
            var contentProperty = metadataProvider.Get(type).ContentProperty;
            if (propertyElements.Any() && contentProperty != null)
            {
                yield return new MemberAssignment
                {
                    Member = Member.FromStandard(type, contentProperty),
                    Children = propertyElements.Select(e => createFunc(e, annotator)).ToList(),
                };
            }
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
            var member = resolver.ResolveProperty(type, propertyElement);
            var children = propertyElement.Elements().Select(e => createFunc(e, annotator));

            var directValue = GetDirectValue(propertyElement);

            return new MemberAssignment { Member = member, Children = children, SourceValue = directValue };
        }

        private static string GetDirectValue(XContainer node)
        {
            var nodeFirstNode = node.FirstNode;
            if (nodeFirstNode != null && nodeFirstNode.NodeType == XmlNodeType.Text)
            {
                return ((XText)nodeFirstNode).Value;
            }

            return null;
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
            return !(attribute.IsNamespaceDeclaration || (attribute.Name.Namespace == SpecialNamespace));
        }

        private MemberAssignment ToAssignment(Type type, XAttribute attribute)
        {
            var value = attribute.Value;
            var property = resolver.ResolveProperty(type, attribute);

            var inlineParser = inlineParsers.FirstOrDefault(p => p.CanParse(value));
            if (inlineParser != null)
            {
                var constructionNode = inlineParser.Parse(value, prefix => GetNamespaceFromPrefix(attribute, prefix));
                return new MemberAssignment { Member = property, Children = new[] { constructionNode } };
            }

            var assignment = new MemberAssignment
            {
                Member = property,
                SourceValue = value
            };

            return assignment;
        }

        private static string GetNamespaceFromPrefix(XAttribute attribute, string prefix)
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