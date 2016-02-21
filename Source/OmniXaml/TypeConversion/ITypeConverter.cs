namespace OmniXaml.TypeConversion
{
    using System;
    using System.Globalization;

    public interface ITypeConverter
    {
        object ConvertFrom(IValueContext context, CultureInfo culture, object value);

        // ReSharper disable once UnusedMember.Global
        object ConvertTo(IValueContext context, CultureInfo culture, object value, Type destinationType);

        // ReSharper disable once UnusedMember.Global
        bool CanConvertTo(IValueContext context, Type destinationType);

        // ReSharper disable once UnusedMember.Global
        bool CanConvertFrom(IValueContext context, Type sourceType);
    }
}