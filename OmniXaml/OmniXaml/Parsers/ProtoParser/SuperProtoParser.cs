namespace OmniXaml.Parsers.ProtoParser
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
            foreach (var protoXamlNode in CommonNodesOfElement(xamlType, emptyElement)) yield return protoXamlNode;
        }

        private IEnumerable<ProtoXamlNode> CommonNodesOfElement(XamlType owner, ProtoXamlNode elementToInject)
        {
            var rawAttributes = GetAttributes(owner).ToList();

            foreach (var node in GetPrefixDefinitions(rawAttributes).Select(ConvertAttributeToNsPrefixDefinition)) yield return node;

            yield return elementToInject;

            foreach (var node in GetAttributes(rawAttributes).Select(ConvertAttributeToNode)) yield return node;
        }

        private IEnumerable<ProtoXamlNode> ParseExpandedElement(XamlType xamlType)
        {
            var element = nodeBuilder.NonEmptyElement(xamlType.UnderlyingType, wiringContext.TypeContext.GetNamespaceForPrefix(reader.Prefix));
            foreach (var node in CommonNodesOfElement(xamlType, element)) yield return node;

            reader.Read();

            if (reader.NodeType != XmlNodeType.EndElement)
            {
                SkipWhitespaces();

                var memberName = GetMemberName(reader.LocalName);
                yield return nodeBuilder.NonEmptyPropertyElement(xamlType.UnderlyingType, memberName, "root");

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

        private IEnumerable<RawAttribute> GetAttributes(IEnumerable<RawAttribute> rawAttributes)
        {
            return rawAttributes.Where(attribute => !attribute.Name.Contains("xmlns"));
        }

        private ProtoXamlNode ConvertAttributeToNode(RawAttribute rawAttribute)
        {
            var member = rawAttribute.Owner.GetMember(rawAttribute.Name);
            return nodeBuilder.Attribute(member, rawAttribute.Value);
        }

        private IEnumerable<RawAttribute> GetPrefixDefinitions(IEnumerable<RawAttribute> rawAttributes)
        {
            return rawAttributes.Where(attribute => attribute.Name.Contains("xmlns"));
        }

        private IEnumerable<RawAttribute> GetAttributes(XamlType owner)
        {
            if (reader.MoveToFirstAttribute())
            {
                do
                {
                    yield return new RawAttribute(owner, reader.Name, reader.Value);
                } while (reader.MoveToNextAttribute());

                reader.MoveToElement();
            }
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

        private static string GetMemberName(string propertyName)
        {
            var indexOfDot = propertyName.IndexOf(".") + 1;
            return propertyName.Substring(indexOfDot, propertyName.Length - indexOfDot);
        }

        private void SkipWhitespaces()
        {
            while (reader.NodeType == XmlNodeType.Whitespace)
            {
                reader.Read();
            }
        }

        private XamlType CurrentType => wiringContext.TypeContext.GetByPrefix(reader.Prefix, reader.LocalName);

        private ProtoXamlNode ConvertAttributeToNsPrefixDefinition(RawAttribute rawAttribute)
        {
            var value = rawAttribute.Value;
            var propName = new Property(rawAttribute.Name);
            return nodeBuilder.NamespacePrefixDeclaration(value, propName.Name);
        }
    }

    internal class RawAttribute
    {
        public XamlType Owner { get; }
        public string Name { get; }
        public string Value { get; }

        public RawAttribute(XamlType owner, string name, string value)
        {
            this.Owner = owner;
            this.Name = name;
            this.Value = value;
        }
    }
}