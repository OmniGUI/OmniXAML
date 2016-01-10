namespace OmniXaml
{
    using System;
    using System.IO;
    using Parsers.ProtoParser;

    public class XmlLoader : ILoader
    {
        private readonly IParserFactory parserFactory;
        private IXmlReader xmlReader;

        public XmlLoader(IParserFactory parserFactory)
        {
            this.parserFactory = parserFactory;
        }

        public object Load(Stream stream)
        {
            return Load(stream, parserFactory.CreateForReadingFree());
        }

        public object Load(Stream stream, object instance)
        {
            return Load(stream, parserFactory.CreateForReadingSpecificInstance(instance));
        }

        public object Load(Stream stream, LoadSettings loadSettings)
        {
            var parser = loadSettings.RootInstance == null
                ? parserFactory.CreateForReadingFree()
                : parserFactory.CreateForReadingSpecificInstance(loadSettings.RootInstance);

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