namespace OmniXaml.Wpf.Loader
{
    using System.IO;

    public class WpfXamlLoader : IXamlLoader
    {
        private readonly XamlXmlLoader xamlXmlLoader;

        public WpfXamlLoader()
        {
            var wiringContext = WpfWiringContextFactory.Create();
            var objectAssembler = new WpfObjectAssembler(wiringContext);

            xamlXmlLoader = new XamlXmlLoader(objectAssembler, wiringContext);
        }

        public object Load(Stream stream)
        {
            return xamlXmlLoader.Load(stream);
        }

        public object Load(Stream stream, object rootInstance)
        {
            return xamlXmlLoader.Load(stream, rootInstance);
        }
    }
}