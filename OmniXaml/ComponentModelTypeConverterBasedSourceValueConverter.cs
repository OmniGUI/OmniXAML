using System;
using System.ComponentModel;
using Serilog;
using Zafiro.Core;

namespace OmniXaml
{
    public class ComponentModelTypeConverterBasedSourceValueConverter : IStringSourceValueConverter
    {
        private static readonly Type StringType = typeof(string);

        public (bool, object) Convert(string strValue, Type targetType, ConvertContext context)
        {
            if (targetType.IsAssignableFrom(typeof(string)))
            {
                return (true, strValue);
            }

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