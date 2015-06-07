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
        private readonly ISet<XamlNamespace> newNamespaces = new HashSet<XamlNamespace>(); 

        public IEnumerable<string> RegisteredPrefixes => registeredPrefixes.Keys;

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            registeredPrefixes.Add(prefixRegistration.Prefix, prefixRegistration.Ns);
        }

        public XamlNamespace GetXamlNamespace(string ns)
        {
            var xamlNamespace = newNamespaces.FirstOrDefault(ns1 => ns1.Name == ns);
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

        public void AddNamespace(XamlNamespace xamlNamespace)
        {
            newNamespaces.Add(xamlNamespace);
        }

        public void AddCatalog(AttributeBasedClrMappingCatalog attributeBasedClrMappingCatalog)
        {
           throw new NotImplementedException();
        }
    }
}