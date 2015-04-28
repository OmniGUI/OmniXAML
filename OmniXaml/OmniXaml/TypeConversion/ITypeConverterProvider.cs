namespace OmniXaml.TypeConversion
{
    using System;
    using System.Collections.Generic;

    public interface ITypeConverterProvider
    {
        ITypeConverter GetTypeConverter(Type type);
        
        void AddCatalog(IDictionary<Type, ITypeConverter> typeConverters);
    }
}