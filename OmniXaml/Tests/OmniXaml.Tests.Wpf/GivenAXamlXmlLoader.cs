namespace OmniXaml.Tests.Wpf
{
    using System.IO;
    using System.Text;
    using Assembler;
    using OmniXaml.Wpf.Loader;

    public class GivenAXamlXmlLoader : GivenAWiringContext
    {
        protected GivenAXamlXmlLoader()
        {
            Loader = new WpfXamlLoader();
        }

        private IXamlLoader Loader { get; }
        
        protected object LoadXaml(string xamlContent)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xamlContent)))
            {
                return Loader.Load(stream);
            }
        }
    }
}