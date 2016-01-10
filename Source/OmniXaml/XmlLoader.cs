namespace OmniXaml
{
    using System;
    using System.IO;
    using ObjectAssembler;
    using Parsers.ProtoParser;

    public class XmlLoader : ILoader
    {
        private readonly IParserFactory parserFactory;
        private IXmlReader xmlReader;

        public XmlLoader(IParserFactory parserFactory)
        {
            this.parserFactory = parserFactory;
        }

        public object Load(Stream stream, Settings loadSettings)
        {
            var parser = parserFactory.Create(loadSettings);
            return Load(stream, parser);
        }

        private object Load(Stream stream, IParser parser)
        {
            try
            {
                xmlReader = new XmlCompatibilityReader(stream);
                return parser.Parse(xmlReader);
            }
            catch (Exception e)
            {
                throw new LoadException($"Error loading XAML: {e}", xmlReader.LineNumber, xmlReader.LinePosition, e);
            }
        }
    }
}