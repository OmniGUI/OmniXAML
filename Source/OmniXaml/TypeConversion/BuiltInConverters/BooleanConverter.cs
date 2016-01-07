namespace OmniXaml.TypeConversion.BuiltInConverters
{
    using System;
    using System.Globalization;

    public class BooleanConverter : ITypeConverter
    {
        public object ConvertFrom(ITypeConverterContext context, CultureInfo culture, object value)
        {
            return bool.Parse((string) value);
        }

        public object ConvertTo(ITypeConverterContext context, CultureInfo culture, object value, Type destinationType)
        {
            return value.ToString();
        }

        public bool CanConvertTo(ITypeConverterContext context, Type destinationType)
        {
            return true;
        }

        public bool CanConvertFrom(ITypeConverterContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }
    }
}