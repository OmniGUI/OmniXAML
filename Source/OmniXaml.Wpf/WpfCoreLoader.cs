namespace OmniXaml.Wpf
{
    internal class WpfCoreLoader : XmlLoader
    {
        public WpfCoreLoader(ITypeFactory typeFactory) : base(new WpfParserFactory())
        {
        }
    }
}