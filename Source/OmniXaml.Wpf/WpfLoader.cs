namespace OmniXaml.Wpf
{
    using System.IO;

    public class WpfLoader : ILoader
    {
        private readonly XmlLoader innerLoader;

        public WpfLoader()
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