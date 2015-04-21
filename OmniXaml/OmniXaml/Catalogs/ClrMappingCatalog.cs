namespace OmniXaml.Catalogs
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ClrMappingCatalog
    {
        public IEnumerable<ClrMapping> Mappings => new ReadOnlyCollection<ClrMapping>(InternalMappings.ToList());

        protected HashSet<ClrMapping> InternalMappings { get; } = new HashSet<ClrMapping>();
    }
}