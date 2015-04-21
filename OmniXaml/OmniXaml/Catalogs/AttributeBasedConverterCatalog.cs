namespace OmniXaml.Catalogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TypeConversion;

    public class AttributeBasedConverterCatalog : Dictionary<Type, ITypeConverter>
    {
        public AttributeBasedConverterCatalog(IEnumerable<Assembly> assemblies)
        {
            var types = assemblies.SelectMany(assembly => assembly.DefinedTypes).ToList();

            var typeAndAttributePairs = from typeAndAttributePair in
                                            from type in types
                                            let att = type.GetCustomAttribute<TypeConverterAttribute>()
                                            select new { type, att }
                                        where typeAndAttributePair.att != null
                                        select typeAndAttributePair;

            foreach (var typeWithAttribute in typeAndAttributePairs)
            {
                var converterInstance = (ITypeConverter)Activator.CreateInstance(typeWithAttribute.att.Converter);
                Add(typeWithAttribute.type.AsType(), converterInstance);
            }
        }
    }
}