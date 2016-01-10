namespace OmniXaml.Wpf
{
    using System.IO;
    using OmniXaml.ObjectAssembler;

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

        public object Load(Stream stream, Settings settings)
        {
            return innerLoader.Load(stream, settings);
        }
    }
}