namespace OmniXaml.Tests.Wpf
{
    using Classes;

    public class GivenAWiringContext
    {
        protected WiringContext WiringContext => OmniXaml.Wpf.WpfWiringContextFactory.Create();
    }
}