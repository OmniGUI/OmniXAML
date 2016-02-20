namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Common.NetCore;

    public class GivenAXmlLoader : GivenARuntimeTypeSourceNetCore
    {
        protected GivenAXmlLoader()
        {
            Loader = new XmlLoader(new DummyParserFactory(RuntimeTypeSource));
        }

        protected XmlLoader Loader { get; }
    }
}
