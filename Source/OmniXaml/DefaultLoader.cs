namespace OmniXaml
{
    using System.IO;
    using ObjectAssembler;

    public class DefaultLoader : ILoader
    {
        private readonly XmlLoader xmlLoader;

        public DefaultLoader(IRuntimeTypeSource runtimeTypeSource)
        {
            IParserFactory parserFactory = new DefaultParserFactory(runtimeTypeSource);
            xmlLoader = new XmlLoader(parserFactory);
        }

        public object Load(Stream stream, Settings settings)
        {
            return xmlLoader.Load(stream, settings);
        }
    }
}