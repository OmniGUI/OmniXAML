namespace OmniXaml.Wpf
{
    using System.IO;
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class WpfXamlStreamLoader : IXamlStreamLoader
    {
        private readonly BoostrappableXamlStreamLoader coreBoostrappableXamlStreamLoader;

        public WpfXamlStreamLoader()
        {
            var wiringContext = WiringContextFactory.Create();
            var factory = new ObjectAssemblerFactory(wiringContext);
            coreBoostrappableXamlStreamLoader = new BoostrappableXamlStreamLoader(
                wiringContext,
                new SuperProtoParser(wiringContext),
                new XamlNodesPullParser(wiringContext), 
                factory);
        }

        public object Load(Stream stream)
        {
            return coreBoostrappableXamlStreamLoader.Load(stream);
        }

        public object Load(Stream stream, object rootInstance)
        {
            return coreBoostrappableXamlStreamLoader.Load(stream, rootInstance);
        }
    }
}