namespace OmniXaml.Typing
{
    using Builder;

    public interface IXamlNamespaceRegistry
    {
        string GetNamespaceForPrefix(string prefix);
        void RegisterPrefix(PrefixRegistration prefixRegistration);
        void AddNamespace(XamlNamespace xamlNamespace);
        XamlNamespace GetXamlNamespace(string ns);
    }
}