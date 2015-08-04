namespace OmniXaml
{
    using System;
    using Builder;
    using Catalogs;
    using TypeConversion;

    public class TypeFeatureProvider : ITypeFeatureProvider
    {
        public TypeFeatureProvider(IContentPropertyProvider contentPropertyProvider, ITypeConverterProvider converterProvider)
        {
            ContentPropertyProvider = contentPropertyProvider;
            ConverterProvider = converterProvider;
        }

        public IContentPropertyProvider ContentPropertyProvider { get; }

        public ITypeConverterProvider ConverterProvider { get; }
        public ITypeConverter GetTypeConverter(Type type)
        {
            return ConverterProvider.GetTypeConverter(type);
        }

        public void RegisterConverter(TypeConverterRegistration typeConverterRegistration)
        {
            ConverterProvider.RegisterConverter(typeConverterRegistration);
        }

        public string GetContentPropertyName(Type type)
        {
            return ContentPropertyProvider.GetContentPropertyName(type);
        }

        public void AddCatalog(ContentPropertyCatalog catalog)
        {
            ContentPropertyProvider.AddCatalog(catalog);
        }

        public void Add(ContentPropertyDefinition contentPropertyDefinition)
        {
            ContentPropertyProvider.Add(contentPropertyDefinition);
        }
    }
}