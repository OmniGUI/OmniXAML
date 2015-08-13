namespace OmniXaml.Wpf
{
    public class WpfXamlLoader : XamlLoader
    {
        public WpfXamlLoader(ITypeFactory typeFactory) : base(new WpfParserFactory(typeFactory))
        {
        }
    }
}