namespace OmniXaml
{
    using System;
    using System.IO;
    using Parsers.ProtoParser;

    public class XamlXmlLoader : IXamlLoader
    {
        private readonly IXamlParserFactory xamlParserFactory;
        private IXmlReader xmlReader;

        public XamlXmlLoader(IXamlParserFactory xamlParserFactory)
        {
            this.xamlParserFactory = xamlParserFactory;
        }

        public object Load(Stream stream)
        {
            return Load(stream, xamlParserFactory.CreateForReadingFree());
        }

        public object Load(Stream stream, object instance)
        {
            return Load(stream, xamlParserFactory.CreateForReadingSpecificInstance(instance));
        }

        private object Load(Stream stream, IXamlParser parser)
        {
            try
            {
                xmlReader = new XmlCompatibilityReader(stream);
                return parser.Parse(xmlReader);
            }
            catch (Exception e)
            {
                throw new XamlLoadException($"Error loading XAML: {e}", xmlReader.LineNumber, xmlReader.LinePosition, e);
            }
        }
    }
}