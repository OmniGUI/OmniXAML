namespace OmniXaml.TypeConversion.BuiltInConverters
{
    using System;
    using System.Globalization;

    public class BooleanConverter : ITypeConverter
    {
        public object ConvertFrom(IValueContext context, CultureInfo culture, object value)
        {
            return bool.Parse((string) value);
        }

        public object ConvertTo(IValueContext context, CultureInfo culture, object value, Type destinationType)
        {
            return value.ToString();
        }

        public bool CanConvertTo(IValueContext context, Type destinationType)
        {
            return true;
        }

        public bool CanConvertFrom(IValueContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }
    }
}