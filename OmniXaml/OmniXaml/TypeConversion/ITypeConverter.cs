namespace OmniXaml.TypeConversion
{
    using System;
    using System.Globalization;

    public interface ITypeConverter
    {
        object ConvertFrom(CultureInfo culture, object value);

        // ReSharper disable once UnusedMember.Global
        object ConvertTo(CultureInfo culture, object value, Type destinationType);

        // ReSharper disable once UnusedMember.Global
        bool CanConvertTo(Type destinationType);

        // ReSharper disable once UnusedMember.Global
        bool CanConvertFrom(Type sourceType);
    }
}