namespace OmniXaml.Typing
{
    using System.Collections.Generic;
    using System.Linq;
    using Catalogs;

    public class XamlNamespaceRegistry : IXamlNamespaceRegistry
    {
        private readonly IDictionary<string, string> registeredPrefixes = new Dictionary<string, string>();
        private readonly ISet<XamlNamespace> namespaces = new HashSet<XamlNamespace>();

        public IEnumerable<string> RegisteredPrefixes => registeredPrefixes.Keys;

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            registeredPrefixes.Add(prefixRegistration.Prefix, prefixRegistration.Ns);
        }

        public XamlNamespace GetXamlNamespace(string ns)
        {
            var xamlNamespace = namespaces.FirstOrDefault(ns1 => ns1.NamespaceUri == ns);
            return xamlNamespace;
        }

        public string GetNamespaceForPrefix(string prefix)
        {
            return registeredPrefixes[prefix];
        }

        public XamlNamespace GetXamlNamespaceByPrefix(string prefix)
        {
            return GetXamlNamespace(registeredPrefixes[prefix]);
        }

        public void RegisterNamespace(XamlNamespace xamlNamespace)
        {
            namespaces.Add(xamlNamespace);
        }

        public void AddCatalog(AttributeBasedClrMappingCatalog attributeBasedClrMappingCatalog)
        {
            foreach (var xamlMapping in attributeBasedClrMappingCatalog.Mappings.ToLookup(mapping => mapping.XamlNamespace))
            {
                var xamlNs = xamlMapping.Key;
                var mappedTo = xamlMapping.Select(mapping => new ClrAssemblyPair(mapping.Assembly, mapping.ClrNamespace));

                RegisterNamespace(new XamlNamespace(xamlNs, mappedTo));
            }
        }
    }
}