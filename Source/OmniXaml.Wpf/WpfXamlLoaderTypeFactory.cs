namespace OmniXaml.Wpf
{
    public class WpfXamlLoaderTypeFactory : AlternateOnRootTypeFactory
    {
        public WpfXamlLoaderTypeFactory() : base(new TypeFactory(), new WpfTypeFactory())
        {
        }
    }
}