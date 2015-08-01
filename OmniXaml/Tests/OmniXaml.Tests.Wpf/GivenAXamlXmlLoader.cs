namespace OmniXaml.Tests.Wpf
{
    using System.IO;
    using System.Text;
    using Glass;
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
            using (var stream = xamlContent.ToStream())
            {
                return XamlStreamLoader.Load(stream);
            }
        }
    }
}