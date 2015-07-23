namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using OmniXaml.Parsers.ProtoParser.SuperProtoParser;
    using OmniXaml.Parsers.XamlNodes;
    using Tests;

    public class GivenAXamlXmlLoader : GivenAWiringContext
    {
        protected GivenAXamlXmlLoader()
        {
            XamlStreamLoader = new XamlStreamLoader(new SuperProtoParser(WiringContext),
                new XamlNodesPullParser(WiringContext),
                new DefaultObjectAssemblerFactory(WiringContext));
        }

        protected XamlStreamLoader XamlStreamLoader { get; }
    }
}
