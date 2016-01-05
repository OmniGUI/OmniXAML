namespace OmniXaml.Wpf
{
    using System.IO;

    public class WpfXamlLoader : IXamlLoader
    {
        private readonly XamlXmlLoader innerLoader;

        public WpfXamlLoader()
        {
            innerLoader = new XamlXmlLoader(new WpfParserFactory());
        }

        public object Load(Stream stream)
        {
            return innerLoader.Load(stream);
        }

        public object Load(Stream stream, object rootInstance)
        {
            return innerLoader.Load(stream, rootInstance);
        }
    }
}