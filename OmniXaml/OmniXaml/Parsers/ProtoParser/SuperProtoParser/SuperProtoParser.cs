namespace OmniXaml.Parsers.ProtoParser.SuperProtoParser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Typing;

    public class SuperProtoParser
    {
        private readonly WiringContext wiringContext;
        private readonly ProtoNodeBuilder nodeBuilder;
        private IXmlReader reader;

        public SuperProtoParser(WiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
            nodeBuilder = new ProtoNodeBuilder(wiringContext.TypeContext);
        }

        public IEnumerable<ProtoXamlNode> Parse(Stream stream)
        {
            reader = new XmlCompatibilityReader(stream);
            reader.Read();

            return ParseElement();
        }

        private IEnumerable<ProtoXamlNode> ParseEmptyElement(XamlType xamlType, NamespaceDeclaration namespaceDeclaration, AttributeFeed attributes)
        {
            var emptyElement = nodeBuilder.EmptyElement(xamlType.UnderlyingType, namespaceDeclaration);
            return CommonNodesOfElement(xamlType, emptyElement, attributes);
        }

        private IEnumerable<ProtoXamlNode> CommonNodesOfElement(XamlType owner, ProtoXamlNode elementToInject, AttributeFeed attributeFeed)
        {
            var attributes = attributeFeed;

            foreach (var node in attributes.PrefixRegistrations.Select(ConvertAttributeToNsPrefixDefinition)) yield return node;
            
            yield return elementToInject;

            foreach (var node in attributes.Directives.Select(ConvertDirective)) yield return node;
            foreach (var node in attributes.RawAttributes.Select(a => ConvertAttributeToNode(owner, a))) yield return node;
        }

        private IEnumerable<ProtoXamlNode> ParseExpandedElement(XamlType xamlType, NamespaceDeclaration namespaceDeclaration, AttributeFeed attributes)
        {
            var element = nodeBuilder.NonEmptyElement(xamlType.UnderlyingType, namespaceDeclaration);
            foreach (var node in CommonNodesOfElement(xamlType, element, attributes)) yield return node;

            reader.Read();

            foreach (var protoXamlNode in ParseNestedElements(xamlType)) yield return protoXamlNode;

            yield return nodeBuilder.EndTag();
        }

        private IEnumerable<ProtoXamlNode> ParseNestedElements(XamlType xamlType)
        {
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                SkipWhitespaces();

                var isPropertyElement = reader.LocalName.Contains('.');
                if (isPropertyElement)
                {
                    foreach (var node in ParseNestedProperty(xamlType)) yield return node;

                    reader.Read();
                }
                else
                {
                    foreach (var node in ParseChildren()) yield return node;
                }
            }
        }

        private IEnumerable<ProtoXamlNode> ParseNestedProperty(XamlType xamlType)
        {
            var propertyLocator = PropertyLocator.Parse(reader.LocalName);
            var namespaceDeclaration = new NamespaceDeclaration(reader.Namespace, reader.Prefix);
            yield return nodeBuilder.NonEmptyPropertyElement(xamlType.UnderlyingType, propertyLocator.PropertyName, namespaceDeclaration);
            reader.Read();

            foreach (var p in ParseInnerTextIfAny()) yield return p;

            SkipWhitespaces();
            if (reader.NodeType != XmlNodeType.EndElement)
            {
                foreach (var protoXamlNode in ParseChildren())
                {
                    yield return protoXamlNode;
                }
            }

            yield return nodeBuilder.EndTag();
        }

        private IEnumerable<ProtoXamlNode> ParseInnerTextIfAny()
        {
            if (reader.NodeType == XmlNodeType.Text)
            {
                yield return nodeBuilder.Text(reader.Value);
                reader.Read();
            }
        }

        private IEnumerable<ProtoXamlNode> ParseChildren()
        {
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                foreach (var p in ParseElement())
                {
                    yield return p;
                }

                yield return nodeBuilder.Text();

                reader.Read();
                SkipWhitespaces();
            }
        }

        private void AssertValidElement()
        {
            if (!(reader.NodeType == XmlNodeType.Element && !reader.LocalName.Contains(".")))
            {
                throw new XamlParseException("The root should be an element.");
            }
        }

        private ProtoXamlNode ConvertAttributeToNode(XamlType containingType, UnprocessedAttribute rawAttribute)
        {
            MutableXamlMember member;

            if (rawAttribute.Locator.IsDotted)
            {
                var ownerName = rawAttribute.Locator.Owner.PropertyName;
                var ownerPrefix = rawAttribute.Locator.Owner.Prefix;

                var owner = wiringContext.TypeContext.GetByPrefix(ownerPrefix, ownerName);

                member = owner.GetAttachableMember(rawAttribute.Locator.PropertyName);
            }
            else
            {
                member = containingType.GetMember(rawAttribute.Locator.PropertyName);
            }

            var namespaceDeclaration = new NamespaceDeclaration(rawAttribute.Locator.Namespace, rawAttribute.Locator.Prefix);
            return nodeBuilder.Attribute(member, rawAttribute.Value, namespaceDeclaration);
        }

        private AttributeFeed GetAttributes()
        {
            AttributeReader attributeReader = new AttributeReader(reader);
            return attributeReader.Load();
        }

        private IEnumerable<ProtoXamlNode> ParseElement()
        {
            SkipWhitespaces();

            AssertValidElement();

            var attributes = GetAttributes();

            var prefixRegistrations = attributes.PrefixRegistrations.ToList();

            RegisterPrefixes(prefixRegistrations);

            var prefix = reader.Prefix;
            var ns = wiringContext.TypeContext.GetNamespaceByPrefix(prefix);

            var childType = wiringContext.TypeContext.GetByPrefix(prefix, reader.LocalName);
            var namespaceDeclaration = new NamespaceDeclaration(ns.Name, prefix);

            if (reader.IsEmptyElement)
            {                
                foreach (var node in ParseEmptyElement(childType, namespaceDeclaration, attributes)) yield return node;
            }
            else
            {
                foreach (var node in ParseExpandedElement(childType, namespaceDeclaration, attributes)) yield return node;
            }
        }

        private void RegisterPrefixes(IEnumerable<NsPrefix> prefixRegistrations)
        {
            foreach (var prefixRegistration in prefixRegistrations)
            {
                var registration = new PrefixRegistration(prefixRegistration.Prefix, prefixRegistration.Namespace);
                wiringContext.TypeContext.RegisterPrefix(registration);
            }
        }

        private void SkipWhitespaces()
        {
            while (reader.NodeType == XmlNodeType.Whitespace)
            {
                reader.Read();
            }
        }

        private ProtoXamlNode ConvertDirective(RawDirective directive)
        {
            return nodeBuilder.Attribute(CoreTypes.Key, directive.Value, new NamespaceDeclaration("http://schemas.microsoft.com/winfx/2006/xaml", "x"));
        }

        private ProtoXamlNode ConvertAttributeToNsPrefixDefinition(NsPrefix prefix)
        {
            return nodeBuilder.NamespacePrefixDeclaration(prefix.Prefix, prefix.Namespace);
        }
    }
}