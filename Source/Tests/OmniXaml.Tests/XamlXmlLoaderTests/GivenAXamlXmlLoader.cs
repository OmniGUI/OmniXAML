namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Common.NetCore;

    public class GivenAXamlXmlLoader : GivenAWiringContextNetCore
    {
        protected GivenAXamlXmlLoader()
        {
            XamlXmlLoader = new XamlXmlLoader(new DummyXamlParserFactory(WiringContext));
        }

        protected XamlXmlLoader XamlXmlLoader { get; }
    }
}
