namespace OmniXaml.TypeConversion.BuiltInConverters
{
    using System;
    using System.Globalization;

    public class TypeTypeConverter : ITypeConverter
    {
        public bool CanConvertFrom(ITypeConverterContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public object ConvertFrom(ITypeConverterContext context, CultureInfo culture, object value)
        {
            var qualifiedTypeName = value as string;
            return context.TypeRepository.GetByQualifiedName(qualifiedTypeName).UnderlyingType;            
        }

        public bool CanConvertTo(ITypeConverterContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public object ConvertTo(
            ITypeConverterContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
           throw new NotImplementedException();
        }        
    }
}