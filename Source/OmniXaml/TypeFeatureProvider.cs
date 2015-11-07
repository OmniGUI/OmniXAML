namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using Builder;
    using TypeConversion;

    public class TypeFeatureProvider : ITypeFeatureProvider
    {
        public TypeFeatureProvider(IContentPropertyProvider contentPropertyProvider, ITypeConverterProvider converterProvider, IRuntimeNameProvider runtimeNamePropertyProvider)
        {
            ContentPropertyProvider = contentPropertyProvider;
            ConverterProvider = converterProvider;
            RuntimeNamePropertyProvider = runtimeNamePropertyProvider;            
        }

        public IRuntimeNameProvider RuntimeNamePropertyProvider { get; }

        public IContentPropertyProvider ContentPropertyProvider { get; }

        public ITypeConverterProvider ConverterProvider { get; }
        public ITypeConverter GetTypeConverter(Type type)
        {
            return ConverterProvider.GetTypeConverter(type);
        }

        public void AddTypeConverter(TypeConverterRegistration typeConverterRegistration)
        {
            ConverterProvider.Add(typeConverterRegistration);
        }

        public string GetContentPropertyName(Type type)
        {
            return ContentPropertyProvider.GetContentPropertyName(type);
        }

        public void AddContentProperty(ContentPropertyDefinition item)
        {
            ContentPropertyProvider.Add(item);
        }

        public IEnumerable<TypeConverterRegistration> TypeConverters => ConverterProvider;
        public IEnumerable<ContentPropertyDefinition> ContentProperties => ContentPropertyProvider;
        public string GetRuntimeNameProperty(Type type)
        {
            return RuntimeNamePropertyProvider.GetRuntimeNameProperty(type);
        }

        public void RegisterRuntimeNameProperty(Type type, string propertyName)
        {
            RuntimeNamePropertyProvider.Add(new RuntimeNamePropertyRegistration(type, propertyName));
        }
    }
}