namespace OmniXaml.Tests
{
    using System;
    using System.Collections.Generic;
    using Builder;
    using TypeConversion;
    using Typing;

    public class TypeFeatureProviderDummy : ITypeFeatureProvider
    {
        public ITypeConverter GetTypeConverter(Type type)
        {
            throw new NotImplementedException();
        }

        public void AddTypeConverter(TypeConverterRegistration typeConverterRegistration)
        {
            throw new NotImplementedException();
        }

        public string GetContentPropertyName(Type type)
        {
            throw new NotImplementedException();
        }

        public void AddContentProperty(ContentPropertyDefinition item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TypeConverterRegistration> TypeConverters { get; }
        public IEnumerable<ContentPropertyDefinition> ContentProperties { get; }
        public Metadata GetMetadata(XamlType xamlType)
        {
            throw new NotImplementedException();
        }

        public Metadata GetMetadata(Type type)
        {
            throw new NotImplementedException();
        }

        public void RegisterMetadata(Type type, Metadata metadata)
        {
            throw new NotImplementedException();
        }
    }
}