namespace OmniXaml
{
    using TypeConversion;

    public class WiringContext
    {
        public WiringContext(ITypeContext typeContext, IContentPropertyProvider contentPropertyProvider, ITypeConverterProvider converterProvider)
        {
            TypeContext = typeContext;
            ContentPropertyProvider = contentPropertyProvider;
            ConverterProvider = converterProvider;
        }

        public ITypeContext TypeContext { get; private set; }
        public IContentPropertyProvider ContentPropertyProvider { get; private set; }
        public ITypeConverterProvider ConverterProvider { get; private set; }
    }
}