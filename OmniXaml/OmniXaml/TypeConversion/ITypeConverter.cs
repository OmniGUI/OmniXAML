namespace OmniXaml.TypeConversion
{
    using System;
    using System.Globalization;

    public interface ITypeConverter
    {
        object ConvertFrom(CultureInfo culture, object value);

        object ConvertTo(CultureInfo culture, object value, Type destinationType);

        bool CanConvertTo(Type destinationType);

        bool CanConvertFrom(Type sourceType);
    }
}