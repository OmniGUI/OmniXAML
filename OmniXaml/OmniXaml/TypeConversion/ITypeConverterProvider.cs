namespace OmniXaml.TypeConversion
{
    using System;
    using Builder;

    public interface ITypeConverterProvider
    {
        ITypeConverter GetTypeConverter(Type type);
        void RegisterConverter(TypeConverterRegistration typeConverterRegistration);
    }
}