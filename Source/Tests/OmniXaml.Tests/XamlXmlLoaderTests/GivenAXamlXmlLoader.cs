namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Common.NetCore;

    public class GivenAXamlXmlLoader : GivenARuntimeTypeContextNetCore
    {
        protected GivenAXamlXmlLoader()
        {
            Loader = new XmlLoader(new DummyXamlParserFactory(TypeRuntimeTypeSource));
        }

        protected XmlLoader Loader { get; }
    }
}
