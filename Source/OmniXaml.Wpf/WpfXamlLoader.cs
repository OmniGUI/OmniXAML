namespace OmniXaml.Wpf
{
    using System.IO;

    public class WpfXamlLoader : IXamlLoader
    {
        private readonly XmlLoader innerLoader;

        public WpfXamlLoader()
        {
            innerLoader = new XmlLoader(new WpfParserFactory());
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