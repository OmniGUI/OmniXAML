namespace OmniXaml.TypeConversion.BuiltInConverters
{
    using System;
    using System.Globalization;

    public class IntTypeConverter : ITypeConverter
    {
        public object ConvertFrom(CultureInfo culture, object value)
        {
            return int.Parse((string) value);
        }

        public object ConvertTo(CultureInfo culture, object value, Type destinationType)
        {
            return value.ToString();
        }

        public bool CanConvertTo(Type destinationType)
        {
            return true;
        }

        public bool CanConvertFrom(Type sourceType)
        {
            return true;
        }
    }
}