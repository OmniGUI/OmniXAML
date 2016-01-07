namespace OmniXaml
{
    using System.IO;

    internal class DefaultLoader : ILoader
    {
        private readonly IRuntimeTypeSource runtimeTypeSource;
        private readonly XmlLoader xmlLoader;

        public DefaultLoader(IRuntimeTypeSource runtimeTypeSource)
        {
            this.runtimeTypeSource = runtimeTypeSource;
            IParserFactory pfb= new DefaultParserFactory(runtimeTypeSource);
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