namespace SampleOmniXAML
{
    using System.IO;
    using OmniXaml;

    internal class DefaultXamlLoader : IXamlLoader
    {
        private readonly XmlLoader xmlLoader;

        public DefaultXamlLoader(IRuntimeTypeSource typeContext)
        {
            IXamlParserFactory parserFactory= new DefaultParserFactory(typeContext);
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