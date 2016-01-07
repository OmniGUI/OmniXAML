namespace OmniXaml.TypeConversion.BuiltInConverters
{
    using System;
    using System.Globalization;

    public class DoubleTypeConverter : ITypeConverter
    {
        public object ConvertFrom(ITypeConverterContext context, CultureInfo culture, object value)
        {
            return double.Parse((string) value, CultureInfo.InvariantCulture);
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