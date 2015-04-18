namespace OmniXaml.TypeConversion
{
    using System;
    using System.Globalization;

    public interface ITypeConverter
    {
        object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value);

        object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType);

        bool CanConvertTo(ITypeDescriptorContext context, Type destinationType);

        bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType);
    }
}