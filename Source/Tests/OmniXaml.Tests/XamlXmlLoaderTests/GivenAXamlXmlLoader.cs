namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Common.NetCore;

    public class GivenAXamlXmlLoader : GivenAWiringContextNetCore
    {
        protected GivenAXamlXmlLoader()
        {
            XamlLoader = new XamlLoader(new DummyXamlParserFactory(WiringContext));
        }

        protected XamlLoader XamlLoader { get; }
    }
}
