namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    public class SourceValueConverter : ISourceValueConverter
    {
        private readonly Dictionary<Type, Func<ConverterValueContext, object>> standardConverters = new Dictionary<Type, Func<ConverterValueContext, object>>();
        private readonly ISet<TypeConverter> typeConverters = new HashSet<TypeConverter>();

        public object GetCompatibleValue(ConverterValueContext valueContext)
        {
            var targetType = valueContext.TargetType;
            var sourceValue = valueContext.Value as string;

            if (sourceValue == null)
            {
                return valueContext.Value;
            }

            object result;
            if (BuiltItConversionParser.TryParse(targetType, sourceValue, out result))
            {
                return result;
            }

            var delegateParent = valueContext.BuildContext.AmbientRegistrator.Instances.FirstOrDefault();
            if (DelegateParser.TryParse(sourceValue, targetType, delegateParent, out result))
            {
                return result;
            }

            Func<ConverterValueContext, object> converter;
            if (standardConverters.TryGetValue(targetType, out converter))
            {
                return converter(valueContext);
            }

            if (ConvertersTryParse(sourceValue, valueContext, out result))
            {
                return result;
            }

            return valueContext.Value;
        }

        private bool ConvertersTryParse(string sourceValue, ConverterValueContext valueContext, out object result)
        {
            var converter = typeConverters.FirstOrDefault(typeConverter => typeConverter.CanConvertFrom(typeof(string)));
            if (converter != null)
            {
                result = converter.ConvertFrom(new SourceValueConverterTypeDescriptorContext(valueContext), CultureInfo.CurrentCulture, sourceValue);
                return true;
            }

            result = null;
            return false;
        }

        public void Add(Type type, Func<ConverterValueContext, object> func)
        {
            standardConverters.Add(type, func);
        }

        public void Add(TypeConverter converter)
        {
            typeConverters.Add(converter);
        }
    }
}