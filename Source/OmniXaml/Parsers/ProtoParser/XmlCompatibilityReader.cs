namespace OmniXaml.Parsers.ProtoParser
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Xml;

    internal class XmlCompatibilityReader : IXmlReader
    {
        private readonly XmlReader xmlReader;

        public XmlCompatibilityReader(TextReader stringReader)
        {
            xmlReader = XmlReader.Create(stringReader);
        }

        public XmlCompatibilityReader(Stream stream)
        {
            var xmlReaderSettings = new XmlReaderSettings { IgnoreComments = true };
            xmlReader = XmlReader.Create(stream, xmlReaderSettings);
        }

        public void Read()
        {
            xmlReader.Read();
        }

        public XmlNodeType NodeType => xmlReader.NodeType;

        public bool IsEmptyElement => xmlReader.IsEmptyElement;
        public string Prefix => xmlReader.Prefix;
        public string LocalName => xmlReader.LocalName;
        public string Name => xmlReader.Name;
        public string Value => xmlReader.Value;
        public bool MoveToFirstAttribute()
        {
            var hadNext = xmlReader.MoveToFirstAttribute();
            if (ShouldIgnore)
            {
                return xmlReader.MoveToNextAttribute();
            }

            return hadNext;
        }

        public bool MoveToNextAttribute()
        {
            bool hadNext;

            do
            {
                hadNext = xmlReader.MoveToNextAttribute();
            } while (ShouldIgnore && hadNext);

            return hadNext;
        }

        private bool ShouldIgnore => IsDesignerOnly || IsUnsupportedAttribute;
        private bool IsUnsupportedAttribute => UnsupportedAttributes.Contains(Name);
        public string Namespace => xmlReader.NamespaceURI;
        public IEnumerable<string> UnsupportedAttributes { get; } = new Collection<string> { "x:TypeArguments", "x:Class" };

        private bool IsDesignerOnly => xmlReader.Prefix == "d" || Name == "mc:Ignorable";

        public void MoveToElement()
        {
            xmlReader.MoveToElement();
        }
    }
}