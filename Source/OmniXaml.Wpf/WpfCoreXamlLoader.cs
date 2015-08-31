namespace OmniXaml.Wpf
{
    internal class WpfCoreXamlLoader : XamlLoader
    {
        public WpfCoreXamlLoader(ITypeFactory typeFactory) : base(new WpfParserFactory(typeFactory))
        {
        }
    }
}