namespace OmniXaml.Rework
{
    using System;
    using System.ComponentModel;
    using Serilog;

    public class BuiltInConverter : IStringSourceValueConverter
    {
        private static readonly Type StringType = typeof(string);

        public (bool, object) TryConvert(string strValue, Type targetType)
        {
            object converted = null;

            var typeConverter = TypeDescriptor.GetConverter(targetType);

            if (typeConverter.CanConvertFrom(StringType))
            {
                try
                {
                    converted = typeConverter.ConvertFromInvariantString(strValue);
                }
                catch (Exception)
                {
                    Log.Information(@"The built-in converter for the type {TargetType} couldn't convert the string ""{Value}""", targetType, strValue);
                    return (false, converted);
                }

                return (true, converted);
            }

            return (false, null);
        }
    }
}