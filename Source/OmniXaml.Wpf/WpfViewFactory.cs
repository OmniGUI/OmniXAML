namespace OmniXaml.Wpf
{
    using Services.DotNetFx;
    using Services.Mvvm;

    public class WpfViewFactory : ViewFactory
    {
        // ReSharper disable once UnusedMember.Global
        public WpfViewFactory() : this(new TypeFactory())
        {            
        }

        public WpfViewFactory(ITypeFactory typeFactory) : base(new WpfTypeFactory(typeFactory))
        {
            RegisterViews(ViewRegistration.FromTypes(Types.FromCurrentAppDomain));
        }
    }
}