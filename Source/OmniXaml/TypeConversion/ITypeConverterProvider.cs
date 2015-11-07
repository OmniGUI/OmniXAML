namespace OmniXaml.TypeConversion
{
    using System;
    using System.Collections.Generic;
    using Builder;
    using Glass;

    public interface ITypeConverterProvider : IAdd<TypeConverterRegistration>, IEnumerable<TypeConverterRegistration>
    {
        ITypeConverter GetTypeConverter(Type type);        
    }

    public interface IRuntimeNameProvider : IAdd<RuntimeNamePropertyRegistration>, IEnumerable<RuntimeNamePropertyRegistration>
    {
        string GetRuntimeNameProperty(Type type);
    }
}