namespace OmniXaml.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TypeConversion;

    public static class Converters
    {
        public static IEnumerable<TypeConverterRegistration> FromAssemblies(IEnumerable<Assembly> lookupAssemblies)
        {
            var types = lookupAssemblies.SelectMany(assembly => assembly.DefinedTypes).ToList();

            var typeAndAttributePairs = from typeAndAttributePair in
                from type in types
                let att = type.GetCustomAttribute<TypeConverterAttribute>()
                select new { type, att }
                where typeAndAttributePair.att != null
                select typeAndAttributePair;

            foreach (var typeWithAttribute in typeAndAttributePairs)
            {
                var converterInstance = (ITypeConverter)Activator.CreateInstance(typeWithAttribute.att.Converter);
                yield return new TypeConverterRegistration(typeWithAttribute.type.AsType(), converterInstance);
            }
        }
    }
}