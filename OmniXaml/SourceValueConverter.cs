namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class SourceValueConverter : ISourceValueConverter
    {
        readonly Dictionary<Type, Func<ValueContext, object>> converters = new Dictionary<Type, Func<ValueContext, object>>();

        public object GetCompatibleValue(ValueContext valueContext)
        {
            var targetType = valueContext.Assignment.Property.PropertyType;
            var sourceValue = (string)valueContext.Assignment.Value;

            if (targetType == typeof(int))
            {
                return int.Parse(sourceValue);
            }

            if (targetType == typeof(double))
            {
                return int.Parse(sourceValue);
            }

            if (typeof(Delegate).GetTypeInfo().IsAssignableFrom(targetType.GetTypeInfo()) && valueContext.Assignment.Property.IsEvent)
            {
                var rootInstance = valueContext.BuildContext.AmbientRegistrator.Instances.First();
                var callbackMethodInfo = rootInstance.GetType()
                    .GetRuntimeMethods().First(method => method.Name.Equals(sourceValue));
                return callbackMethodInfo.CreateDelegate(valueContext.Assignment.Property.PropertyType, rootInstance);
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

        public void Add(Type type, Func<ValueContext, object> func)
        {
            converters.Add(type, func);
        }
    }
}