namespace OmniXaml.Wpf
{
    internal class WpfCoreXamlLoader : XamlXmlLoader
    {
        public WpfCoreXamlLoader(ITypeFactory typeFactory) : base(new WpfParserFactory(typeFactory))
        {
        }
    }
}