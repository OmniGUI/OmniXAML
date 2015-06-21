namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using global::OmniXaml.Assembler;

    namespace OmniXaml.Reader.Tests.Wpf
    {
        public class GivenAXamlXmlLoader : GivenAWiringContext
        {
            protected GivenAXamlXmlLoader()
            {
                Loader = new XamlXmlLoader(new ObjectAssembler(WiringContext), WiringContext);
            }

            protected XamlXmlLoader Loader { get; }
        }
    }
}