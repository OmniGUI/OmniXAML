namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class SourceValueConverter : ISourceValueConverter
    {
        readonly Dictionary<Type, Func<ConverterValueContext, object>> converters = new Dictionary<Type, Func<ConverterValueContext, object>>();

        public object GetCompatibleValue(ConverterValueContext valueContext)
        {
            var sourceValue = (string)valueContext.Value;
            var targetType = valueContext.TargetType;

            if (targetType == typeof(int))
            {
                return int.Parse(sourceValue);
            }

            if (targetType == typeof(double))
            {
                return int.Parse(sourceValue);
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

            return sourceValue;
        }

       
        public void Add(Type type, Func<ConverterValueContext, object> func)
        {
            converters.Add(type, func);
        }
    }

    public class ConverterValueContext
    {

        public ObjectBuilderContext ObjectBuilderContext { get; }
        public Type TargetType { get; }
        public object Value { get; set; }
        public ITypeDirectory TypeDirectory { get; }
        public BuildContext BuildContext { get; set; }

        public ConverterValueContext(Type targetType, object value, ObjectBuilderContext objectBuilderContext, ITypeDirectory directory, BuildContext buildContext)
        {
            TargetType = targetType;
            Value = value;
            TypeDirectory = directory;
            BuildContext = buildContext;
            ObjectBuilderContext = objectBuilderContext;
        }
    }
}