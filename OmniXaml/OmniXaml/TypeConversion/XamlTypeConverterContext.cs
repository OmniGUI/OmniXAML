namespace OmniXaml.TypeConversion
{
    public class XamlTypeConverterContext : IXamlTypeConverterContext
    {
        public XamlTypeConverterContext(ITypeContext typeContext)
        {
            TypeContext = typeContext;
        }
        public ITypeContext TypeContext { get;  }
    }
}