namespace OmniXaml.TypeConversion.BuiltInConverters
{
    using System;
    using System.Globalization;

    public class IntTypeConverter : ITypeConverter
    {
        public object ConvertFrom(IValueContext context, CultureInfo culture, object value)
        {
            var str = value as string;

            if (value is int)
            {
                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            if (value is long)
            {
                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            else if (str != null)
            {
                long v;
                if (long.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out v))
                {
                    return (int)v;
                }
            }

            throw new InvalidOperationException();
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
            return sourceType == typeof(string) || sourceType == typeof(long) || sourceType == typeof(int);
        }
    }
}