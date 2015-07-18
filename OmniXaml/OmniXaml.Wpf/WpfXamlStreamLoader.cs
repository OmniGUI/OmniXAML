namespace OmniXaml.Wpf
{
    using System.IO;

    public class WpfXamlStreamLoader : IXamlStreamLoader
    {
        private readonly XamlStreamLoader coreXamlStreamLoader;

        public WpfXamlStreamLoader()
        {
            var wiringContext = WiringContextFactory.Create();
            var factory = new ObjectAssemblerFactory(wiringContext);
            coreXamlStreamLoader = new XamlStreamLoader(wiringContext, factory);
        }

        public object Load(Stream stream)
        {
            return coreXamlStreamLoader.Load(stream);
        }

        public object Load(Stream stream, object rootInstance)
        {
            return coreXamlStreamLoader.Load(stream, rootInstance);
        }
    }
}