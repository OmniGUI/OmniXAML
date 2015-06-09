namespace OmniXaml.TypeConversion
{
    using System;

    public interface ITypeConverterProvider
    {
        ITypeConverter GetTypeConverter(Type type);
    }
}