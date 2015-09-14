namespace OmniXaml.ObjectAssembler
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Commands;
    using TypeConversion;
    using Typing;

    public class ValuePipeline
    {
        private readonly IXamlTypeRepository typeRepository;
        private readonly ITopDownValueContext topDownValueContext;

        public ValuePipeline(IXamlTypeRepository typeRepository, ITopDownValueContext topDownValueContext)
        {
            this.typeRepository = typeRepository;
            this.topDownValueContext = topDownValueContext;
        }

        public IXamlTypeRepository TypeRepository => typeRepository;

        public object ConvertValueIfNecessary(object value, XamlType targetType)
        {
            if (IsAlreadyCompatible(value, targetType.UnderlyingType))
            {
                return value;
            }

            object converted;
            var success = TrySpecialConversion(value, targetType.UnderlyingType, out converted);
            if (success)
            {
                return converted;
            }

            var typeConverter = targetType.TypeConverter;
            if (typeConverter != null)
            {
                var context = new XamlTypeConverterContext(typeRepository, topDownValueContext);
                if (typeConverter.CanConvertFrom(context, value.GetType()))
                {
                    var anotherValue = typeConverter.ConvertFrom(context, CultureInfo.InvariantCulture, value);
                    return anotherValue;
                }
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