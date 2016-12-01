namespace OmniXaml
{
    using System;
    using System.ComponentModel;
    using Serilog;

    public static class BuiltInConversionParser
    {
        private static readonly Type StringType = typeof(string);
        
        public static bool TryParse(Type targetType, string sourceValue, out object converted)
        {
            converted = null;

            var typeConverter = TypeDescriptor.GetConverter(targetType);
            
            if (typeConverter.CanConvertFrom(StringType))
            {
                try
                {
                    converted = typeConverter.ConvertFromInvariantString(sourceValue);
                }
                catch (Exception)
                {
                    Log.Information(@"The built-in converter for the type {TargetType} couldn't convert the string ""{Value}""", targetType, sourceValue);
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}