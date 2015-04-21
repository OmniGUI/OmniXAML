namespace OmniXaml.TypeConversion
{
    using System;
    using System.Globalization;

    public class StringTypeConverter : ITypeConverter
    {
        public object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
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

        public object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
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

            return value.ToString();
        }

        public bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(int))
            {
                return true;
            }

            return false;
        }

        public bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }
    }
}