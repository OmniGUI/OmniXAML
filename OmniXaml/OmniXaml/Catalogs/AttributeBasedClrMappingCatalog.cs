namespace OmniXaml.Catalogs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;

    public class AttributeBasedClrMappingCatalog : ClrMappingCatalog
    {
        public AttributeBasedClrMappingCatalog(IEnumerable<Assembly> assemblies)
        {
            var attributes = from assembly in assemblies
                             let attribute = assembly.GetCustomAttribute<XmlnsDefinitionAttribute>()
                             where attribute != null
                             select new { Assembly = assembly, attribute.ClrNamespace, attribute.XmlNamespace };

            foreach (var mapping in attributes)
            {
                InternalMappings.Add(new ClrMapping(mapping.Assembly, mapping.XmlNamespace, mapping.ClrNamespace));
            }
        }
    }
}