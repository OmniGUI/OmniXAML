namespace OmniXaml.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using TypeConversion;
    using ITypeDescriptorContext = System.ComponentModel.ITypeDescriptorContext;

    internal class ConverterAdapter : ITypeConverter
    {
        private readonly TypeConverter converter;

        public ConverterAdapter(TypeConverter converter)
        {
            this.converter = converter;
        }

        public object ConvertFrom(IValueContext context, CultureInfo culture, object value)
        {
            ITypeDescriptorContext typeDescriptor = new TypeDescriptorContext();
            return converter.ConvertFrom(typeDescriptor, culture, value);
        }

        public object ConvertTo(IValueContext context, CultureInfo culture, object value, Type destinationType)
        {
            return converter.ConvertTo(null, culture, value, destinationType);
        }

        public bool CanConvertTo(IValueContext context, Type destinationType)
        {
            return converter.CanConvertTo(null, destinationType);
        }

        public bool CanConvertFrom(IValueContext context, Type sourceType)
        {
            return converter.CanConvertFrom(null, sourceType);
        }
    }    
}