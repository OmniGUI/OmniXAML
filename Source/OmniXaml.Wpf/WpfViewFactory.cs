namespace OmniXaml.Wpf
{
    using AppServices.Mvvm;
    using AppServices.NetCore;

    public class WpfViewFactory : ViewFactory
    {
        public WpfViewFactory(ITypeFactory typeFactory) : base(new WpfTypeFactory(typeFactory))
        {
            RegisterViews(ViewRegistration.FromTypes(Types.FromCurrentAppDomain));
        }
    }
}