namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class XamlNamespace
    {
        private readonly string xamlNamespaceUri;

        private readonly HashSet<ClrAssemblyPair> mappings = new HashSet<ClrAssemblyPair>();

        public XamlNamespace(string xamlNamespaceUri)
        {
            this.xamlNamespaceUri = xamlNamespaceUri;
        }

        public XamlNamespace(string xamlNamespaceUri, IEnumerable<ClrAssemblyPair> clrAssemblyPair) : this(xamlNamespaceUri)
        {
            mappings = new HashSet<ClrAssemblyPair>(clrAssemblyPair);
        }

        public string NamespaceUri => xamlNamespaceUri;

        public void AddMapping(ClrAssemblyPair clrAssemblyPair)
        {
            mappings.Add(clrAssemblyPair);
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