namespace OmniXaml.Wpf
{
    using System.IO;
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class WpfXamlStreamLoader : IXamlStreamLoader
    {
        private readonly BootstrappableXamlStreamLoader coreBootstrappableXamlStreamLoader;

        public WpfXamlStreamLoader()
        {
            var wiringContext = WiringContextFactory.Create();
            var factory = new ObjectAssemblerFactory(wiringContext);
            coreBootstrappableXamlStreamLoader = new BootstrappableXamlStreamLoader(
                wiringContext,
                new SuperProtoParser(wiringContext),
                new XamlNodesPullParser(wiringContext), 
                factory);
        }

        public object Load(Stream stream)
        {
            return coreBootstrappableXamlStreamLoader.Load(stream);
        }

        public object Load(Stream stream, object rootInstance)
        {
            return coreBootstrappableXamlStreamLoader.Load(stream, rootInstance);
        }
    }
}