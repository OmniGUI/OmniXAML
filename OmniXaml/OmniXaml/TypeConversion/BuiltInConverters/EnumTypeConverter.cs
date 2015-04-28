namespace OmniXaml.TypeConversion.BuiltInConverters
{
    using System;
    using System.Globalization;

    internal class EnumTypeConverter : ITypeConverter
    {
        public object ConvertFrom(CultureInfo culture, object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertTo(CultureInfo culture, object value, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public bool CanConvertTo(Type destinationType)
        {
            throw new NotImplementedException();
        }

        public bool CanConvertFrom(Type sourceType)
        {
            throw new NotImplementedException();
        }
    }
}