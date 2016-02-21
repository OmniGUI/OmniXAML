namespace OmniXaml.TypeConversion.BuiltInConverters
{
    using System;
    using System.Globalization;

    public class TypeTypeConverter : ITypeConverter
    {
        public bool CanConvertFrom(IValueContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public object ConvertFrom(IValueContext context, CultureInfo culture, object value)
        {
            var qualifiedTypeName = value as string;
            return context.TypeRepository.GetByQualifiedName(qualifiedTypeName).UnderlyingType;            
        }

        public bool CanConvertTo(IValueContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public object ConvertTo(
            IValueContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
           throw new NotImplementedException();
        }        
    }
}