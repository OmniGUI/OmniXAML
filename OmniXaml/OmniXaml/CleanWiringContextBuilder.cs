namespace OmniXaml
{
    using TypeConversion;
    using Typing;

    public class CleanWiringContextBuilder
    {
        public ITypeContext TypeContext { get; protected set; }
        public IContentPropertyProvider ContentPropertyProvider { get; protected set; }
        public ITypeConverterProvider TypeConverterProvider { get; protected set; }

        public CleanWiringContextBuilder()
        {
            var xamlNamespaceRegistry = new XamlNamespaceRegistry();
            TypeContext = new TypeContext(new XamlTypeRepository(xamlNamespaceRegistry), xamlNamespaceRegistry, new TypeFactory());
            ContentPropertyProvider = new ContentPropertyProvider();
            TypeConverterProvider = new TypeConverterProvider();
        }

        public WiringContext Build()
        {
            return new WiringContext(TypeContext, ContentPropertyProvider, TypeConverterProvider);
        }
    }
}