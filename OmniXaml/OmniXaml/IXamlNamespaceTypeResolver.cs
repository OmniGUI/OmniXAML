namespace OmniXaml
{
    using System;
    using System.Reflection;
    using Catalogs;

    public interface IXamlNamespaceTypeResolver
    {
        Type Resolve(string typeName, string xamlNamespace);
        void AddCatalog(ClrMappingCatalog catalog);        

        void Map(string xamlNs, Assembly assembly, string clrNs);
    }
}