namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using Builder;
    using TypeConversion;
    using Typing;

    public class TypeFeatureProvider : ITypeFeatureProvider
    {
        private readonly MetadataProvider metadatas = new MetadataProvider();

        public TypeFeatureProvider(ITypeConverterProvider converterProvider)
        {
            ConverterProvider = converterProvider;
        }

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
            return metadatas.Get(type).ContentProperty;
        }

        public void AddContentProperty(ContentPropertyDefinition item)
        {
            ContentPropertyProvider.Add(item);
        }

        public IEnumerable<TypeConverterRegistration> TypeConverters => ConverterProvider;
        public IEnumerable<ContentPropertyDefinition> ContentProperties => ContentPropertyProvider;

        public Metadata GetMetadata(Type type)
        {
            return metadatas.Get(type);
        }

        public void RegisterMetadata(Type type, Metadata metadata)
        {
            metadatas.Register(type, metadata);
        }
    }
}