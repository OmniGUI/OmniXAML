namespace OmniXaml
{
    using System.IO;

    internal class DefaultXamlLoader : IXamlLoader
    {
        private readonly ITypeContext wiringContext;
        private readonly XamlXmlLoader xamlXmlLoader;

        public DefaultXamlLoader(ITypeContext wiringContext)
        {
            this.wiringContext = wiringContext;
            IXamlParserFactory pfb= new DefaultParserFactory(wiringContext);
            xamlXmlLoader = new XamlXmlLoader(pfb);
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