namespace OmniXaml.Wpf
{
    using AppServices.Mvvm;
    using AppServices.NetCore;

    public class WpfViewFactory : ViewFactory
    {
        public WpfViewFactory() : base(new WpfInflatableTypeFactory())
        {
            RegisterViews(ViewRegistration.FromTypes(Types.FromCurrentAddDomain));
        }
    }
}