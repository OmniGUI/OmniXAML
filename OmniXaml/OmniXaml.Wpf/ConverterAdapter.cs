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

        public object ConvertFrom(CultureInfo culture, object value)
        {
            ITypeDescriptorContext typeDescriptor = new FakeDescriptor();
            return converter.ConvertFrom(typeDescriptor, culture, value);
        }

        public object ConvertTo(CultureInfo culture, object value, Type destinationType)
        {
            return converter.ConvertTo(null, culture, value, destinationType);
        }

        public bool CanConvertTo(Type destinationType)
        {
            return converter.CanConvertTo(null, destinationType);
        }

        public bool CanConvertFrom(Type sourceType)
        {
            return converter.CanConvertFrom(null, sourceType);
        }
    }

    public class FakeDescriptor : ITypeDescriptorContext
    {
        public object GetService(Type serviceType)
        {
            return null;
        }

        public bool OnComponentChanging()
        {
            throw new NotImplementedException();
        }

        public void OnComponentChanged()
        {
            throw new NotImplementedException();
        }

        public IContainer Container { get; }
        public object Instance { get; }
        public PropertyDescriptor PropertyDescriptor { get; }
    }
}