namespace OmniXaml.TypeConversion
{
    using System;
    using System.Globalization;

    public class NumberTypeConverter : ITypeConverter
    {
        public object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return value.ToString();
        }

        public bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }
    }
}