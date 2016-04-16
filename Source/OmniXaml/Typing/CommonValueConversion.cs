namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using Glass.Core;
    using TypeConversion;

    public static class CommonValueConversion
    {
        static CommonValueConversion()
        {
            ValueConverters.Add(new NonConversionStep());
            ValueConverters.Add(new SpecialTypesStep());
            ValueConverters.Add(new EnumStep());
            ValueConverters.Add(new ConverterStep());
        }

        private static IList<ICommonValueConverter> ValueConverters { get; } = new List<ICommonValueConverter>();

        public static bool TryConvert(object value, XamlType targetType, IValueContext valueContext, out object result)
        {
            result = null;
            foreach (var step in ValueConverters)
            {
                if (step.TryConvert(value, targetType, valueContext, out result))
                {
                    return true;
                }
            }

            return false;
        }

        private class EnumStep : ICommonValueConverter
        {
            public bool TryConvert(object value, XamlType xamlType, IValueContext valueContext, out object result)
            {
                var targetType = xamlType.UnderlyingType;

                result = null;
                if (targetType.IsNullable())
                {
                    var typeInfo = Nullable.GetUnderlyingType(targetType);
                    if (typeInfo != null && typeInfo.GetTypeInfo().IsEnum)
                    {
                        if (EnumExtensions.TryParse(typeInfo, value.ToString(), out result))
                        {
                            return true;
                        }
                    }
                }

                if (targetType.GetTypeInfo().IsEnum)
                {
                    if (EnumExtensions.TryParse(targetType, value.ToString(), out result))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private interface ICommonValueConverter
        {
            bool TryConvert(object value, XamlType targetType, IValueContext valueContext, out object result);
        }

        private class SpecialTypesStep : ICommonValueConverter
        {
            public bool TryConvert(object value, XamlType targetType, IValueContext valueContext, out object result)
            {
                var type = value.GetType();

                if (type == typeof(string) && targetType.UnderlyingType == typeof(string))
                {
                    result = value;
                    return true;
                }

                if (type == typeof(string) && targetType.UnderlyingType == typeof(object))
                {
                    result = value.ToString();
                    return true;
                }

                result = null;
                return false;
            }
        }

        private class NonConversionStep : ICommonValueConverter
        {
            public bool TryConvert(object value, XamlType targetType, IValueContext valueContext, out object result)
            {
                result = null;

                if (value == null)
                {
                    return true;
                }

                var isAssignable = targetType.UnderlyingType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo());

                if (isAssignable)
                {
                    result = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private class ConverterStep : ICommonValueConverter
        {
            public bool TryConvert(object value, XamlType targetType, IValueContext valueContext, out object result)
            {
                result = null;

                var typeConverter = targetType.TypeConverter;
                if (typeConverter != null)
                {
                    if (typeConverter.CanConvertFrom(valueContext, value.GetType()))
                    {
                        {
                            result = typeConverter.ConvertFrom(valueContext, CultureInfo.InvariantCulture, value);
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}