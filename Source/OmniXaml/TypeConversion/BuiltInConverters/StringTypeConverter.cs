namespace OmniXaml.TypeConversion.BuiltInConverters
{
    using System;
    using System.Globalization;

    public class StringTypeConverter : ITypeConverter
    {
        public object ConvertFrom(IValueContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return value;
            }

            return null;
        }

        public object ConvertTo(IValueContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                return null;
            }

            if (destinationType == typeof(int))
            {
                var str = value as string;
                if (str != null)
                {
                    int n;
                    if (int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out n))
                    {
                        return n;
                    }
                    return null;
                }
            }

            if (destinationType == typeof(double))
            {
                var str = value as string;
                if (str != null)
                {
                    double n;
                    if (double.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out n))
                    {
                        return n;
                    }
                    return null;
                }
            }

            return value.ToString();
        }

        public bool CanConvertTo(IValueContext context, Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(int))
            {
                return true;
            }

            return false;
        }

        public bool CanConvertFrom(IValueContext context, Type sourceType)
        {
            return sourceType == typeof(int) || sourceType == typeof(double) || sourceType == typeof(float);
        }
    }
}