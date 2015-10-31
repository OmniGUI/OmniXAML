namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Common.NetCore;

    public class GivenAXamlXmlLoader : GivenAWiringContextNetCore
    {
        protected GivenAXamlXmlLoader()
        {
            XamlLoader = new XamlXmlLoader(new DummyXamlParserFactory(WiringContext));
        }

        protected XamlXmlLoader XamlLoader { get; }
    }
}
