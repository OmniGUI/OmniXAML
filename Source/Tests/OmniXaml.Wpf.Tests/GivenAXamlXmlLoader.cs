namespace OmniXaml.Wpf.Tests
{
    using Wpf;

    public class GivenAXamlXmlLoader
    {
        protected object LoadXaml(string xamlContent)
        {
            var p = new WpfXamlLoader();
            return p.Load(xamlContent);
        }
    }
}