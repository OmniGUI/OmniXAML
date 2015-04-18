namespace OmniXaml.Catalogs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class ContentPropertyCatalog
    {
        public IDictionary<Type, string> Mappings => new ReadOnlyDictionary<Type, string>(InternalMappings);

        protected IDictionary<Type, string> InternalMappings { get; } = new Dictionary<Type, string>();
    }
}