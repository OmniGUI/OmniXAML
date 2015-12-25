namespace SampleOmniXAML
{
    using System.IO;
    using OmniXaml;

    internal class DefaultXamlLoader : IXamlLoader
    {
        private readonly XamlXmlLoader xamlXmlLoader;

        public DefaultXamlLoader(IWiringContext wiringContext)
        {
            IXamlParserFactory parserFactory= new DefaultParserFactory(wiringContext);
            xamlXmlLoader = new XamlXmlLoader(parserFactory);
        }

        public object Load(Stream stream)
        {
            return xamlXmlLoader.Load(stream);
        }

        public object Load(Stream stream, object instance)
        {
            return xamlXmlLoader.Load(stream, instance);
        }
    }
}