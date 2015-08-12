namespace OmniXaml.Builder
{
    using System;
    using TypeConversion;

    public class TypeConverterRegistration
    {
        private readonly Type type;
        private readonly ITypeConverter typeConverter;

        public TypeConverterRegistration(Type type, ITypeConverter typeConverter)
        {
            this.type = type;
            this.typeConverter = typeConverter;
        }

        public Type TargetType => type;

        public ITypeConverter TypeConverter => typeConverter;
    }
}