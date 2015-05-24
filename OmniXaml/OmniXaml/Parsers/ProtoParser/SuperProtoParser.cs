namespace OmniXaml.Parsers.ProtoParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using Glass;
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

            foreach (var protoXamlNode in ParseNamespaceDefinitions(reader)) yield return protoXamlNode;

            var xamlType = GetCurrentType(reader);

            if (reader.IsEmptyElement)
            {
                yield return nodeBuilder.EmptyElement(xamlType.UnderlyingType, wiringContext.TypeContext.GetNamespaceForPrefix(reader.Prefix));
            }
            else
            {
                foreach (var protoXamlNode in ParseNonEmptyElement(xamlType, reader)) yield return protoXamlNode;
            }
        }

        private IEnumerable<ProtoXamlNode> ParseNonEmptyElement(XamlType xamlType, XmlReader reader)
        {
            yield return
                nodeBuilder.NonEmptyElement(
                    xamlType.UnderlyingType,
                    wiringContext.TypeContext.GetNamespaceForPrefix(reader.Prefix));

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

        private IEnumerable<ProtoXamlNode> ParseChild(XmlReader reader)
        {
            SkipWhitespaces(reader);

            var childType = GetCurrentType(reader);

            if (reader.IsEmptyElement)
            {
                yield return nodeBuilder.EmptyElement(childType.UnderlyingType, "root");
                yield return nodeBuilder.Text();
                yield return nodeBuilder.EndTag();
            }
            else
            {
                foreach (var protoXamlNode in ParseNonEmptyElement(childType, reader)) yield return protoXamlNode;
                yield return nodeBuilder.Text();
                yield return nodeBuilder.EndTag();
            }
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

        private IEnumerable<ProtoXamlNode> ParseNamespaceDefinitions(XmlReader reader)
        {
            if (reader.MoveToFirstAttribute())
            {
                do
                {
                    var value = reader.Value;
                    var propName = new Property(reader.Name);
                    yield return nodeBuilder.NamespacePrefixDeclaration(value, propName.Name);
                } while (reader.MoveToNextAttribute());

                reader.MoveToElement();
            }
        }
    }

    internal class Property
    {
        public Property(string longName)
        {
            Guard.ThrowIfNull(longName, nameof(longName));

            var colonPosition = longName.IndexOf(":", StringComparison.Ordinal);
            if (colonPosition != -1)
            {
                Prefix = longName.Substring(0, colonPosition);
                Name = longName.Substring(colonPosition + 1, longName.Length - colonPosition - 1);
            }
            else
            {
                Prefix = longName;
                Name = string.Empty;
            }
        }

        public string Name { get; }
        public string Prefix { get; private set; }
    }
}