namespace OmniXaml.Builder
{
    using System;
    using TypeConversion;

    public class ConverterRegistration
    {
        private readonly Type type;
        private readonly ITypeConverter typeConverter;

        public ConverterRegistration(Type type, ITypeConverter typeConverter)
        {
            this.type = type;
            this.typeConverter = typeConverter;
        }

        public Type TargetType => type;

        public ITypeConverter TypeConverter => typeConverter;
    }
}