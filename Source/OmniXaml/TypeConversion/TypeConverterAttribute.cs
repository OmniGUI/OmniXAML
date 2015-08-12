namespace OmniXaml.TypeConversion
{
    using System;

    public class TypeConverterAttribute : Attribute
    {
        public Type Converter { get; }

        public TypeConverterAttribute(Type typeConverter)
        {
            Converter = typeConverter;
        }        
    }
}