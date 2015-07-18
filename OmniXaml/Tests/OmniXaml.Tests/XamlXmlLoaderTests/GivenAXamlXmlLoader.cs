namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    namespace OmniXaml.Reader.Tests.Wpf
    {
        public class GivenAXamlXmlLoader : GivenAWiringContext
        {
            protected GivenAXamlXmlLoader()
            {
                XamlStreamLoader = new XamlStreamLoader(WiringContext, new DefaultObjectAssemblerFactory(WiringContext));
            }

            protected XamlStreamLoader XamlStreamLoader { get; }
        }
    }
}