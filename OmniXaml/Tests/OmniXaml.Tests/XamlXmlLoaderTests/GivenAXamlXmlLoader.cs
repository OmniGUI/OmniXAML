namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using global::OmniXaml.Assembler;

    namespace OmniXaml.Reader.Tests.Wpf
    {
        using NewAssembler;

        public class GivenAXamlXmlLoader : GivenAWiringContext
        {
            protected GivenAXamlXmlLoader()
            {
                Loader = new XamlXmlLoader(WiringContext);
            }

            protected XamlXmlLoader Loader { get; }
        }
    }
}