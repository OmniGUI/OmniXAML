namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Builder;
    using Catalogs;

    public class XamlNamespaceRegistry : IXamlNamespaceRegistry
    {
        private readonly IDictionary<string, string> registeredPrefixes = new Dictionary<string, string>();
        private readonly ISet<FullyConfiguredMapping> newNamespaces = new HashSet<FullyConfiguredMapping>(); 

        public IEnumerable<string> RegisteredPrefixes => registeredPrefixes.Keys;

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            registeredPrefixes.Add(prefixRegistration.Prefix, prefixRegistration.Ns);
        }

        public FullyConfiguredMapping GetXamlNamespace(string ns)
        {
            var xamlNamespace = newNamespaces.FirstOrDefault(ns1 => ns1.XamlNamespace == ns);
            return xamlNamespace;
        }

        public string GetNamespaceForPrefix(string prefix)
        {
            return registeredPrefixes[prefix];
        }

        public FullyConfiguredMapping GetXamlNamespaceByPrefix(string prefix)
        {
            return GetXamlNamespace(registeredPrefixes[prefix]);
        }

        public void AddNamespace(FullyConfiguredMapping xamlNamespace)
        {
            newNamespaces.Add(xamlNamespace);
        }

        public void AddCatalog(AttributeBasedClrMappingCatalog attributeBasedClrMappingCatalog)
        {
           throw new NotImplementedException();
        }
    }
}