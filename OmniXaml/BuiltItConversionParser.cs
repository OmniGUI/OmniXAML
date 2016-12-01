namespace OmniXaml
{
    using System;
    using System.ComponentModel;

    public static class BuiltItConversionParser
    {
        private static readonly Type StringType = typeof(string);

        public static bool TryParse(Type targetType, string sourceValue, out object converted)
        {
            converted = null;

            var typeConverter = TypeDescriptor.GetConverter(targetType);
            if (typeConverter.CanConvertFrom(StringType))
            {
                converted = typeConverter.ConvertFrom(sourceValue);
                return true;
            }

            return false;
        }
    }
}