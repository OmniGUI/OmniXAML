namespace SampleOmniXAML
{
    using System.IO;
    using OmniXaml;

    internal class DefaultLoader : ILoader
    {
        private readonly XmlLoader xmlLoader;

        public DefaultLoader(IRuntimeTypeSource typeSource)
        {
            IParserFactory parserFactory= new DefaultParserFactory(typeSource);
            xmlLoader = new XmlLoader(parserFactory);
        }

        public object Load(Stream stream)
        {
            return xmlLoader.Load(stream);
        }

        public object Load(Stream stream, object instance)
        {
            return xmlLoader.Load(stream, instance);
        }
    }
}