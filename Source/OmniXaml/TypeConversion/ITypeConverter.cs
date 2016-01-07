namespace OmniXaml.TypeConversion
{
    using System;
    using System.Globalization;

    public interface ITypeConverter
    {
        object ConvertFrom(ITypeConverterContext context, CultureInfo culture, object value);

        // ReSharper disable once UnusedMember.Global
        object ConvertTo(ITypeConverterContext context, CultureInfo culture, object value, Type destinationType);

        // ReSharper disable once UnusedMember.Global
        bool CanConvertTo(ITypeConverterContext context, Type destinationType);

        // ReSharper disable once UnusedMember.Global
        bool CanConvertFrom(ITypeConverterContext context, Type sourceType);
    }
}