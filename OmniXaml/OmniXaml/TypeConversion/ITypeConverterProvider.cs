namespace OmniXaml.TypeConversion
{
    using System;
    using System.Collections.Generic;

    public interface ITypeConverterProvider
    {
        ITypeConverter GetTypeConverter(Type getType);
        
        void AddCatalog(IDictionary<Type, ITypeConverter> typeConverters);
    }
}