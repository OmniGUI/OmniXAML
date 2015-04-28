namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Catalogs;

    public class DefaultXamlNamespaceTypeResolver : IXamlNamespaceTypeResolver
    {
        private readonly HashSet<ClrMapping> xamlMappings = new HashSet<ClrMapping>();

        public Type Resolve(string typeName, string requestedXamlNs)
        {
            var mapping = xamlMappings.FirstOrDefault(clrMapping => clrMapping.XamlNamespace == requestedXamlNs);
            if (mapping == null)
            {
                throw new TypeNotFoundException(string.Format("The type {0} has not been found in the namespace {1}", typeName, requestedXamlNs));
            }

            return mapping.Assembly.GetType(mapping.ClrNamespace + "." + typeName);
        }

        public void AddCatalog(ClrMappingCatalog catalog)
        {
            throw new NotImplementedException();
        }

        public void Map(string xamlNs, Assembly assembly, string clrNs)
        {
            xamlMappings.Add(new ClrMapping(assembly, xamlNs, clrNs));
        }
    }
}