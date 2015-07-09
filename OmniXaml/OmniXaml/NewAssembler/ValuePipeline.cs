namespace OmniXaml.NewAssembler
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using TypeConversion;

    public class ValuePipeline
    {
        public ValuePipeline(WiringContext wiringContext)
        {
            WiringContext = wiringContext;
        }

        public WiringContext WiringContext { get; }

        public object ConvertValueIfNecessary(object value, Type targetType)
        {
            if (IsAlreadyCompatible(value, targetType))
            {
                return value;
            }

            object converted;
            var success = TrySpecialConversion(value, targetType, out converted);
            if (success)
            {
                return converted;
            }

            var typeConverter = WiringContext.ConverterProvider.GetTypeConverter(targetType);
            if (typeConverter != null)
            {
                var context = new XamlTypeConverterContext(WiringContext.TypeContext);
                var anotherValue = typeConverter.ConvertFrom(context, CultureInfo.InvariantCulture, value);
                return anotherValue;
            }

            return value;
        }

        private static bool IsAlreadyCompatible(object value, Type targetType)
        {
            return targetType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo());
        }

        private static bool TrySpecialConversion(object value, Type targetType, out object converted)
        {
            var type = value.GetType();

            if (type == typeof(string) && targetType == typeof(string))
            {
                converted = value;
                return true;
            }

            if (type == typeof(string) && targetType == typeof(object))
            {
                converted = value.ToString();
                return true;
            }

            if (targetType.GetTypeInfo().IsEnum)
            {
                converted = Enum.Parse(targetType, value.ToString());
                return true;
            }

            converted = null;
            return false;
        }
    }
}