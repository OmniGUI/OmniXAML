namespace OmniXaml.Catalogs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;

    public class AttributeBasedContentPropertyCatalog : ContentPropertyCatalog
    {
        public AttributeBasedContentPropertyCatalog(IEnumerable<Assembly> assemblies)
        {
            var types = assemblies.SelectMany(assembly => assembly.DefinedTypes).ToList();

            var typeAndAttributePairs = from typeAndAttributePair in
                from type in types
                let att = type.GetCustomAttribute<ContentPropertyAttribute>()
                select new { type, att }
                where typeAndAttributePair.att != null
                select typeAndAttributePair;

            var typeWithAttributes = typeAndAttributePairs.ToList();
            foreach (var typeWithAttribute in typeWithAttributes)
            {                
                InternalMappings.Add(typeWithAttribute.type.AsType(), typeWithAttribute.att.Name);
            }
        }
    }
}