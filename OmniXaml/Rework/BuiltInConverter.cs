namespace OmniXaml.Rework
{
    using System;
    using System.ComponentModel;
    using Serilog;

    internal class BuiltInConverter : ITaggedSmartSourceValueConverter
    {
        private static readonly Type StringType = typeof(string);

        public (bool, object, string) TryConvert(string strValue, Type targetType)
        {
            object converted = null;
            var tag = this.GetType().Name;

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
                    return (false, converted, tag);
                }

                return (true, converted, tag);
            }

            return (false, null, tag);
        }
    }

    internal interface ITaggedSmartSourceValueConverter
    {
        (bool, object, string) TryConvert(string strValue, Type targetType);
    }
}