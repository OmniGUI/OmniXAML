namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using Builder;
    using TypeConversion;
    using Typing;

    public interface ITypeFeatureProvider 
    {
        ITypeConverter GetTypeConverter(Type type);
        string GetContentPropertyName(Type type);
        IEnumerable<TypeConverterRegistration> TypeConverters { get; }
        IEnumerable<ContentPropertyDefinition> ContentProperties { get; }
        Metadata GetMetadata(Type type);
        void RegisterMetadata(Type type, Metadata metadata);
    }
}