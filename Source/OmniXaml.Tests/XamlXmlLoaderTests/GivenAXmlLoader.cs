namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Common;
    using Common.DotNetFx;

    public class GivenAXmlLoader : GivenARuntimeTypeSource
    {
        protected GivenAXmlLoader()
        {
            Loader = new XmlLoader(new DummyParserFactory(RuntimeTypeSource));
        }

        protected XmlLoader Loader { get; }
    }
}
