namespace OmniXaml.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Builder;
    using TypeConversion;

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

        public void Add(TypeConverterRegistration item)
        {
            throw new NotImplementedException();
        }     
    }
}