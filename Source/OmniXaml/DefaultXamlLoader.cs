namespace OmniXaml
{
    using System.IO;

    internal class DefaultXamlLoader : IXamlLoader
    {
        private readonly IRuntimeTypeSource runtimeTypeContext;
        private readonly XmlLoader xmlLoader;

        public DefaultXamlLoader(IRuntimeTypeSource runtimeTypeContext)
        {
            this.runtimeTypeContext = runtimeTypeContext;
            IXamlParserFactory pfb= new DefaultParserFactory(runtimeTypeContext);
            xmlLoader = new XmlLoader(pfb);
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