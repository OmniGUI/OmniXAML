namespace OmniXaml.Tests
{
    using System;
    using Builder;
    using TypeConversion;

    public class TypeFeatureProviderDummy : ITypeFeatureProvider
    {
        public ITypeConverter GetTypeConverter(Type type)
        {
            throw new NotImplementedException();
        }

        public void RegisterConverter(TypeConverterRegistration typeConverterRegistration)
        {
            throw new NotImplementedException();
        }

        public string GetContentPropertyName(Type type)
        {
            throw new NotImplementedException();
        }

        public void Add(ContentPropertyDefinition contentPropertyDefinition)
        {
            throw new NotImplementedException();
        }
    }
}