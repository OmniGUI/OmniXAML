namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    namespace OmniXaml.Reader.Tests.Wpf
    {
        public class GivenAXamlXmlLoader : GivenAWiringContext
        {
            protected GivenAXamlXmlLoader()
            {
                Loader = new XamlLoader(WiringContext, new DefaultObjectAssemblerFactory(WiringContext));
            }

            protected XamlLoader Loader { get; }
        }
    }
}