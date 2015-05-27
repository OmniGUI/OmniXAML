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

        public SuperProtoParser(WiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
            nodeBuilder = new ProtoNodeBuilder(wiringContext.TypeContext);
        }

        public IEnumerable<ProtoXamlNode> Parse(string xml)
        {
            var reader = XmlReader.Create(new StringReader(xml));
            reader.Read();

            AssertValidElement(reader);

            var xamlType = GetCurrentType(reader);

            if (reader.IsEmptyElement)
            {
                foreach (var protoXamlNode in ParseEmptyElement(xamlType, reader)) yield return protoXamlNode;
            }
            else
            {
                foreach (var protoXamlNode in ParseElement(xamlType, reader)) yield return protoXamlNode;
            }
        }

        private IEnumerable<ProtoXamlNode> ParseEmptyElement(XamlType xamlType, XmlReader reader)
        {
            var emptyElement = nodeBuilder.EmptyElement(xamlType.UnderlyingType, wiringContext.TypeContext.GetNamespaceForPrefix(reader.Prefix));
            foreach (var protoXamlNode in CommonNodesOfElement(xamlType, reader, emptyElement)) yield return protoXamlNode;
        }

        private IEnumerable<ProtoXamlNode> CommonNodesOfElement(XamlType xamlType, XmlReader reader, ProtoXamlNode elementNode)
        {
            var rawAttributes = GetAttributes(xamlType, reader).ToList();

            foreach (var rawAttribute in GetPrefixDefinitions(rawAttributes))
            {
                yield return ConvertToNsDefinition(rawAttribute);
            }

            yield return elementNode;

            foreach (var p in GetAttributes(rawAttributes).Select(ConvertToAttribute))
            {
                yield return p;
            }
        }

        private IEnumerable<ProtoXamlNode> ParseElement(XamlType xamlType, XmlReader reader)
        {
            var element = nodeBuilder.NonEmptyElement(xamlType.UnderlyingType, wiringContext.TypeContext.GetNamespaceForPrefix(reader.Prefix));
            foreach (var node in CommonNodesOfElement(xamlType, reader, element)) yield return node;

            reader.Read();

            if (reader.NodeType != XmlNodeType.EndElement)
            {
                SkipWhitespaces(reader);

                var memberName = GetMemberName(reader.LocalName);
                yield return nodeBuilder.NonEmptyPropertyElement(xamlType.UnderlyingType, memberName, "root");
                reader.Read();

                foreach (var p in ParseChild(reader)) yield return p;
            }

            yield return nodeBuilder.EndTag();
        }

        private static void AssertValidElement(XmlReader reader)
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

        private ProtoXamlNode ConvertToAttribute(RawAttribute rawAttribute)
        {
            var member = rawAttribute.Owner.GetMember(rawAttribute.Name);
            return nodeBuilder.Attribute(member, rawAttribute.Value);
        }

        private IEnumerable<RawAttribute> GetPrefixDefinitions(IEnumerable<RawAttribute> rawAttributes)
        {
            return rawAttributes.Where(attribute => attribute.Name.Contains("xmlns"));
        }

        private static IEnumerable<RawAttribute> GetAttributes(XamlType owner, XmlReader reader)
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

        private IEnumerable<ProtoXamlNode> ParseChild(XmlReader reader)
        {
            SkipWhitespaces(reader);

            var childType = GetCurrentType(reader);

            if (reader.IsEmptyElement)
            {
                foreach (var protoXamlNode in ParseEmptyElement(childType, reader)) yield return protoXamlNode;                
            }
            else
            {
                foreach (var protoXamlNode in ParseElement(childType, reader)) yield return protoXamlNode;
            }

            yield return nodeBuilder.Text();
            yield return nodeBuilder.EndTag();
        }

        private static string GetMemberName(string propertyName)
        {
            var indexOfDot = propertyName.IndexOf(".") + 1;
            return propertyName.Substring(indexOfDot, propertyName.Length - indexOfDot);
        }

        private static void SkipWhitespaces(XmlReader reader)
        {
            while (reader.NodeType == XmlNodeType.Whitespace)
            {
                reader.Read();
            }
        }

        private XamlType GetCurrentType(XmlReader reader)
        {
            return wiringContext.TypeContext.GetByPrefix(reader.Prefix, reader.LocalName);
        }

        private ProtoXamlNode ConvertToNsDefinition(RawAttribute rawAttribute)
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