namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class SourceValueConverter : ISourceValueConverter
    {
        readonly Dictionary<Type, Func<ValueContext, object>> converters = new Dictionary<Type, Func<ValueContext, object>>();

        public object GetCompatibleValue(ValueContext valueContext, Type targetType)
        {
            var sourceValue = (string)valueContext.Assignment.Value;

            if (targetType == typeof(int))
            {
                return int.Parse(sourceValue);
            }

            if (targetType == typeof(double))
            {
                return int.Parse(sourceValue);
            }

            Func<ValueContext, object> converter;
            if (converters.TryGetValue(targetType, out converter))
            {
                return converter(valueContext);
            }

            if (targetType.GetTypeInfo().IsEnum)
            {
                return Enum.Parse(targetType, sourceValue);
            }

            return sourceValue;
        }

        public object GetCompatibleValue(ValueContext valueContext)
        {
            var targetType = valueContext.Assignment.Property.PropertyType;
            return GetCompatibleValue(valueContext, targetType);
        }

        public void Add(Type type, Func<ValueContext, object> func)
        {
            converters.Add(type, func);
        }
    }
}