namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Common;
    using Common.NetCore;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlNodes;

    public class GivenAXamlXmlLoader : GivenAWiringContextNetCore
    {
        protected GivenAXamlXmlLoader()
        {
            XamlLoader = new XamlLoader(new DummyXamlParserFactory(WiringContext));
        }

        protected XamlLoader XamlLoader { get; }
    }
}
