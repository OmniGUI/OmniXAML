namespace OmniXaml.Typing
{
    using Builder;
    using Catalogs;

    public interface IXamlNamespaceRegistry
    {
        string GetNamespaceForPrefix(string prefix);
        void RegisterPrefix(PrefixRegistration prefixRegistration);
        void AddNamespace(FullyConfiguredMapping xamlNamespace);
        void AddCatalog(AttributeBasedClrMappingCatalog attributeBasedClrMappingCatalog);
        FullyConfiguredMapping GetXamlNamespace(string ns);
    }
}