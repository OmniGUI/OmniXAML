namespace OmniXaml.TypeConversion.BuiltInConverters
{
    using System;
    using System.Globalization;

    public class StringTypeConverter : ITypeConverter
    {
        public object ConvertFrom(CultureInfo culture, object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is string)
            {
                return value;
            }

            return value.ToString();
        }

        public object ConvertTo(CultureInfo culture, object value, Type destinationType)
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
                    if (int.TryParse(str, out n))
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
                    if (double.TryParse(str, out n))
                    {
                        return n;
                    }
                    return null;
                }
            }

            return value.ToString();
        }

        public bool CanConvertTo(Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(int))
            {
                return true;
            }

            return false;
        }

        public bool CanConvertFrom(Type sourceType)
        {
            return true;
        }
    }
}