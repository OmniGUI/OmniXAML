using OmniXaml.TypeConversion;

namespace Perspex.Markup.Test
{
    using System;
    using System.Globalization;

    public class TypeConverterDummy : ITypeConverter
    {
        public object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            throw new NotImplementedException();
        }
    }
}
