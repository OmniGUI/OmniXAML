namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class SourceValueConverter : ISourceValueConverter
    {
        readonly Dictionary<Type, Func<string, object>> converters = new Dictionary<Type, Func<string, object>>();

        public object GetCompatibleValue(Type targetType, string sourceValue)
        {
            if (targetType == typeof(int))
            {
                return int.Parse(sourceValue);
            }

            if (targetType == typeof(double))
            {
                return int.Parse(sourceValue);
            }

            Func<string, object> converter;
            if (converters.TryGetValue(targetType, out converter))
            {
                return converter(sourceValue);
            }

            if (targetType.GetTypeInfo().IsEnum)
            {
                return Enum.Parse(targetType, sourceValue);
            }

            return sourceValue;
        }

        public void Add(Type type, Func<string, object> func)
        {
            converters.Add(type, func);
        }
    }
}