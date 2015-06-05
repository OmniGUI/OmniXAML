namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class XamlNamespace
    {
        private readonly string xamlNamespaceUri;

        private readonly HashSet<ClrNamespaceAddress> mappings = new HashSet<ClrNamespaceAddress>();

        public XamlNamespace(string xamlNamespaceUri)
        {
            this.xamlNamespaceUri = xamlNamespaceUri;
        }

        public XamlNamespace(string xamlNamespaceUri, IEnumerable<ClrNamespaceAddress> clrAssemblyPair) : this(xamlNamespaceUri)
        {
            mappings = new HashSet<ClrNamespaceAddress>(clrAssemblyPair);
        }

        public string NamespaceUri => xamlNamespaceUri;

        public void AddMapping(ClrNamespaceAddress clrNamespaceAddress)
        {
            mappings.Add(clrNamespaceAddress);
        }

        public Type Get(string name)
        {
            var types = from mapping in mappings
                        let t = mapping.Assembly.GetType(mapping.Namespace + "." + name)
                        where t != null
                        select t;

            return types.FirstOrDefault();
        }
    }
}