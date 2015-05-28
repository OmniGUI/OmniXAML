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
            var emptyElement = nodeBuilder.EmptyElement(xamlType.UnderlyingType, wiringContext.TypeContext.GetNamespaceForPrefix(reader.Prefix));
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
            var element = nodeBuilder.NonEmptyElement(xamlType.UnderlyingType, wiringContext.TypeContext.GetNamespaceForPrefix(reader.Prefix));
            foreach (var node in CommonNodesOfElement(xamlType, element)) yield return node;

            reader.Read();

            if (reader.NodeType != XmlNodeType.EndElement)
            {
                SkipWhitespaces();

                var propertyLocator = PropertyLocator.Parse(reader.LocalName);
                yield return nodeBuilder.NonEmptyPropertyElement(xamlType.UnderlyingType, propertyLocator.PropertyName, "root");

                reader.Read();

                foreach (var p in ParseElement()) yield return p;

                yield return nodeBuilder.Text();
                yield return nodeBuilder.EndTag();
            }

            yield return nodeBuilder.EndTag();
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
            
            return nodeBuilder.Attribute(member, rawAttribute.Value);
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

        private ProtoXamlNode ConvertAttributeToNsPrefixDefinition(NsPrefix nsPrefox)
        {
            return nodeBuilder.NamespacePrefixDeclaration(nsPrefox.Namespace, nsPrefox.Prefix);
        }
    }
}