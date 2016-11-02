namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class SourceValueConverter : ISourceValueConverter
    {
        readonly Dictionary<Type, Func<string, SuperContext, object>> converters = new Dictionary<Type, Func<string, SuperContext, object>>();

        public object GetCompatibleValue(SuperContext superContext, Assignment assignment)
        {
            var targetType = assignment.Property.PropertyType;
            var sourceValue = (string)assignment.Value;

            if (targetType == typeof(int))
            {
                return int.Parse(sourceValue);
            }

            if (targetType == typeof(double))
            {
                return int.Parse(sourceValue);
            }

            Func<string, SuperContext, object> converter;
            if (converters.TryGetValue(targetType, out converter))
            {
                return converter(sourceValue, superContext);
            }

            if (targetType.GetTypeInfo().IsEnum)
            {
                return Enum.Parse(targetType, sourceValue);
            }

            return sourceValue;
        }

        public void Add(Type type, Func<string, SuperContext, object> func)
        {
            converters.Add(type, func);
        }
    }
}