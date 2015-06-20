namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using System.IO;
    using System.Text;
    using Assembler;

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