namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class SourceValueConverter : ISourceValueConverter
    {
        private readonly Dictionary<Type, Func<ConverterValueContext, object>> converters = new Dictionary<Type, Func<ConverterValueContext, object>>();

        public object GetCompatibleValue(ConverterValueContext valueContext)
        {
            var targetType = valueContext.TargetType;
            var sourceValue = valueContext.Value as string;

            if (sourceValue == null)
            {
                return valueContext.Value;
            }

            object result;
            if (PrimitiveParser.TryParse(targetType, sourceValue, out result))
            {
                return result;                
            }

            if (DelegateParser.TryParse(sourceValue, targetType, valueContext.BuildContext.AmbientRegistrator.Instances.FirstOrDefault(), out result))
            {
                return result;
            }
            
            Func<ConverterValueContext, object> converter;
            if (converters.TryGetValue(targetType, out converter))
            {
                return converter(valueContext);
            }

            if (targetType.GetTypeInfo().IsEnum)
            {
                return Enum.Parse(targetType, sourceValue);
            }

            return valueContext.Value;
        }

        public void Add(Type type, Func<ConverterValueContext, object> func)
        {
            converters.Add(type, func);
        }
    }
}