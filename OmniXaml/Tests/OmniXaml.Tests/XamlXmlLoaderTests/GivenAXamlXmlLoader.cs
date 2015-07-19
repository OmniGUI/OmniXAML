namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    namespace OmniXaml.Reader.Tests.Wpf
    {
        using global::OmniXaml.Parsers.ProtoParser.SuperProtoParser;
        using global::OmniXaml.Parsers.XamlNodes;

        public class GivenAXamlXmlLoader : GivenAWiringContext
        {
            protected GivenAXamlXmlLoader()
            {
                BootstrappableXamlStreamLoader = new BootstrappableXamlStreamLoader(new SuperProtoParser(WiringContext),
                    new XamlNodesPullParser(WiringContext),
                    new DefaultObjectAssemblerFactory(WiringContext));
            }

            protected BootstrappableXamlStreamLoader BootstrappableXamlStreamLoader { get; }
        }
    }
}