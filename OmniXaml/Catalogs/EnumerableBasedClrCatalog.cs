namespace OmniXaml.Catalogs
{
    using System.Collections.Generic;

    public class EnumerableBasedClrCatalog : ClrMappingCatalog
    {
        public EnumerableBasedClrCatalog(IEnumerable<ClrMapping> mappings)
        {
            foreach (var clrMapping in mappings)
            {
                InternalMappings.Add(clrMapping);
            }
        }
    }
}