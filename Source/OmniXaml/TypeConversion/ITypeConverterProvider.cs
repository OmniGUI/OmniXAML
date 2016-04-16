namespace OmniXaml.TypeConversion
{
    using System;
    using System.Collections.Generic;
    using Builder;
    using Glass.Core;

    public interface ITypeConverterProvider : IAdd<TypeConverterRegistration>, IEnumerable<TypeConverterRegistration>
    {
        ITypeConverter GetTypeConverter(Type type);        
    }
}