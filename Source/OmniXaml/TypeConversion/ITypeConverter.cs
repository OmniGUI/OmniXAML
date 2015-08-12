namespace OmniXaml.TypeConversion
{
    using System;
    using System.Globalization;

    public interface ITypeConverter
    {
        object ConvertFrom(IXamlTypeConverterContext context, CultureInfo culture, object value);

        // ReSharper disable once UnusedMember.Global
        object ConvertTo(IXamlTypeConverterContext context, CultureInfo culture, object value, Type destinationType);

        // ReSharper disable once UnusedMember.Global
        bool CanConvertTo(IXamlTypeConverterContext context, Type destinationType);

        // ReSharper disable once UnusedMember.Global
        bool CanConvertFrom(IXamlTypeConverterContext context, Type sourceType);
    }
}