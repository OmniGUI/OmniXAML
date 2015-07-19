namespace OmniXaml.Tests.Wpf
{
    using System.IO;
    using System.Text;
    using OmniXaml.Wpf;

    public class GivenAXamlXmlLoader
    {
        protected GivenAXamlXmlLoader()
        {
            XamlStreamLoader = new WpfXamlStreamLoader(new TypeFactory());
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