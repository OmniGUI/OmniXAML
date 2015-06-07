namespace OmniXaml.Typing
{
    using Builder;
    using Catalogs;

    public interface IXamlNamespaceRegistry
    {
        string GetNamespaceForPrefix(string prefix);
        void RegisterPrefix(PrefixRegistration prefixRegistration);
        void AddNamespace(XamlNamespace xamlNamespace);
        void AddCatalog(AttributeBasedClrMappingCatalog attributeBasedClrMappingCatalog);
        XamlNamespace GetXamlNamespace(string ns);
    }
}