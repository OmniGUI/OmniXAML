namespace OmniXaml.Wpf
{
    using System.IO;

    public class WpfXamlLoader : IXamlLoader
    {
        private readonly XamlLoader innerLoader;

        public WpfXamlLoader()
        {
            var typeFactory = new TypeFactory();
            innerLoader = new XamlLoader(new WpfParserFactory(new AlternateOnRootTypeFactory(typeFactory, new WpfTypeFactory(typeFactory))));
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