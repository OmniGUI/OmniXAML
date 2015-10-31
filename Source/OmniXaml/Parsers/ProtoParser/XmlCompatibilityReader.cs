namespace OmniXaml.Parsers.ProtoParser
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Xml;

    public class XmlCompatibilityReader : IXmlReader
    {
        private readonly XmlReaderSettings settings = new XmlReaderSettings {IgnoreComments = true};
        private readonly XmlReader xmlReader;

        public XmlCompatibilityReader(TextReader stringReader)
        {
            xmlReader = XmlReader.Create(stringReader, settings);
        }

        public XmlCompatibilityReader(Stream stream)
        {
            xmlReader = XmlReader.Create(stream, settings);
        }

        private bool ShouldIgnore => IsDesignerOnly || IsUnsupportedAttribute;
        private bool IsUnsupportedAttribute => UnsupportedAttributes.Contains(Name);

        public IEnumerable<string> UnsupportedAttributes { get; } = new Collection<string> {"x:TypeArguments", "x:Class"};

        private bool IsDesignerOnly => xmlReader.Prefix == "d" || Name == "mc:Ignorable";

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
        public bool HasLineInfo() => ((IXmlLineInfo) xmlReader).HasLineInfo();
        public int LineNumber => ((IXmlLineInfo) xmlReader).LineNumber;
        public int LinePosition => ((IXmlLineInfo) xmlReader).LinePosition;

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

        public string Namespace => xmlReader.NamespaceURI;


        public void MoveToElement()
        {
            xmlReader.MoveToElement();
        }
    }
}