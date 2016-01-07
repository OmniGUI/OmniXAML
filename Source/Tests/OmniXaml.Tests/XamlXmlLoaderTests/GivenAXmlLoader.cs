namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Common.NetCore;

    public class GivenAXmlLoader : GivenARuntimeTypeSourceNetCore
    {
        protected GivenAXmlLoader()
        {
            Loader = new XmlLoader(new DummyParserFactory(TypeRuntimeTypeSource));
        }

        protected XmlLoader Loader { get; }
    }
}
