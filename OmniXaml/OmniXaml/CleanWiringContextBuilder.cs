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
            var typeFactory = new TypeFactory();
            TypeContext = new TypeContext(new XamlTypeRepository(xamlNamespaceRegistry, typeFactory), xamlNamespaceRegistry, typeFactory);
            ContentPropertyProvider = new ContentPropertyProvider();
            TypeConverterProvider = new TypeConverterProvider();
        }

        public WiringContext Build()
        {
            return new WiringContext(TypeContext, new TypeFeatureProvider(ContentPropertyProvider, TypeConverterProvider));
        }
    }
}