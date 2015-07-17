namespace OmniXaml.Wpf
{
    using System.IO;

    public class WpfLoader : ILoader
    {
        private readonly XamlLoader coreLoader;

        public WpfLoader()
        {
            var wiringContext = WpfWiringContextFactory.Create();
            var factory = new WpfObjectFactory(wiringContext);
            coreLoader = new XamlLoader(wiringContext, factory);
        }

        public object Load(Stream stream)
        {
            return coreLoader.Load(stream);
        }

        public object Load(Stream stream, object rootInstance)
        {
            return coreLoader.Load(stream, rootInstance);
        }
    }
}