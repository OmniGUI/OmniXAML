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

            private XamlXmlLoader Loader { get; }

            protected object LoadXaml(string xamlContent)
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xamlContent)))
                {
                    return Loader.Load(stream);
                }
            }
        }
    }
}