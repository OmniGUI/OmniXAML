namespace OmniXaml.Parsers.ProtoParser.SuperProtoParser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Typing;

    internal class SuperProtoParser
    {
        private readonly WiringContext wiringContext;
        private readonly ProtoNodeBuilder nodeBuilder;
        private XmlReader reader;

        public SuperProtoParser(WiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
            nodeBuilder = new ProtoNodeBuilder(wiringContext.TypeContext);
        }

        public IEnumerable<ProtoXamlNode> Parse(string xml)
        {
            reader = XmlReader.Create(new StringReader(xml));
            reader.Read();

            return ParseElement();
        }

        private IEnumerable<ProtoXamlNode> ParseEmptyElement(XamlType xamlType)
        {
            var emptyElement = nodeBuilder.EmptyElement(xamlType.UnderlyingType, "");
            return CommonNodesOfElement(xamlType, emptyElement);
        }

        private IEnumerable<ProtoXamlNode> CommonNodesOfElement(XamlType owner, ProtoXamlNode elementToInject)
        {
            var attributes = GetAttributes();

            foreach (var node in attributes.PrefixRegistrations.Select(ConvertAttributeToNsPrefixDefinition)) yield return node;

            yield return elementToInject;

            foreach (var node in attributes.RawAttributes.Select(a => ConvertAttributeToNode(owner, a))) yield return node;
        }

        private IEnumerable<ProtoXamlNode> ParseExpandedElement(XamlType xamlType)
        {
            if (xamlType.IsUnreachable)
            {
                throw new XamlReaderException($"The type {xamlType} is unknown, therefore it cannot be reflected.");
            }

            var prefix = string.Empty;
            var ns = wiringContext.TypeContext.GetNamespace(reader.Prefix);
            var element = nodeBuilder.NonEmptyElement(xamlType.UnderlyingType, prefix);
            foreach (var node in CommonNodesOfElement(xamlType, element)) yield return node;

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
            yield return nodeBuilder.NonEmptyPropertyElement(xamlType.UnderlyingType, propertyLocator.PropertyName, propertyLocator.Prefix);
            reader.Read();

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
            XamlMember member;

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

            return nodeBuilder.Attribute(member, rawAttribute.Value, rawAttribute.Locator.Prefix);
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

            var childType = CurrentType;

            if (reader.IsEmptyElement)
            {
                foreach (var node in ParseEmptyElement(childType)) yield return node;
            }
            else
            {
                foreach (var node in ParseExpandedElement(childType)) yield return node;
            }
        }

        private void SkipWhitespaces()
        {
            while (reader.NodeType == XmlNodeType.Whitespace)
            {
                reader.Read();
            }
        }

        private XamlType CurrentType => wiringContext.TypeContext.GetByPrefix(reader.Prefix, reader.LocalName);

        private ProtoXamlNode ConvertAttributeToNsPrefixDefinition(NsPrefix prefix)
        {
            return nodeBuilder.NamespacePrefixDeclaration(prefix.Prefix, prefix.Namespace);
        }
    }
}