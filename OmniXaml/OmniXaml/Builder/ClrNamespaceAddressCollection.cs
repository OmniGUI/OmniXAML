namespace OmniXaml.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Typing;

    public class ClrNamespaceAddressCollection : Collection<ClrNamespaceAddress>
    {
        public ClrNamespaceAddressCollection(IEnumerable<ClrNamespaceAddress> enumerable) : base(enumerable.ToList())
        {
        }

        public Type Get(string typeName)
        {
            var types = from mapping in Items
                let t = mapping.Assembly.GetType(mapping.Namespace + "." + typeName)
                where t != null
                select t;

            return types.FirstOrDefault();
        }
    }
}