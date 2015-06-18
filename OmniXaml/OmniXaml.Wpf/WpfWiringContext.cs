namespace OmniXaml.Wpf
{
    using TypeConversion;

    public class WpfWiringContext : WiringContext
    {
        public WpfWiringContext(ITypeContext typeContext, IContentPropertyProvider contentPropertyProvider, ITypeConverterProvider converterProvider)
            : base(typeContext, contentPropertyProvider, converterProvider)
        {
        }
    }
}