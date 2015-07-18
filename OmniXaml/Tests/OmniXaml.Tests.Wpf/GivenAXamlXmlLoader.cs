namespace OmniXaml.Tests.Wpf
{
    using System.IO;
    using System.Text;
    using OmniXaml.Wpf;

    public class GivenAXamlXmlLoader : GivenAWiringContext
    {
        protected GivenAXamlXmlLoader()
        {
            XamlStreamLoader = new WpfXamlStreamLoader();
        }

        private IXamlStreamLoader XamlStreamLoader { get; }
        
        protected object LoadXaml(string xamlContent)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xamlContent)))
            {
                return XamlStreamLoader.Load(stream);
            }
        }
    }
}