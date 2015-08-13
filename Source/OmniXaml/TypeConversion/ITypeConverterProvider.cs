namespace OmniXaml.TypeConversion
{
    using System;
    using System.Collections.Generic;
    using Builder;

    public interface ITypeConverterProvider : IAdd<TypeConverterRegistration>, IEnumerable<TypeConverterRegistration>
    {
        ITypeConverter GetTypeConverter(Type type);        
    }
}