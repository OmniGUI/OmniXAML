namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using Builder;
    using Glass.Core;
    using TypeConversion;

    public static class TypeConverterProviderMixin
    {
        public static ITypeConverterProvider FillFromAttributes(this ITypeConverterProvider converterProvider, IEnumerable<Type> types)
        {
            var defs = Extensions.GatherAttributes<TypeConverterAttribute, TypeConverterRegistration>(
                types,
                (type, attribute) => new TypeConverterRegistration(type, CreateConverterInstance(attribute)));

            converterProvider.AddAll(defs);
            return converterProvider;
        }

        private static ITypeConverter CreateConverterInstance(TypeConverterAttribute attribute)
        {
            return (ITypeConverter)Activator.CreateInstance(attribute.Converter, null);
        }
    }
}