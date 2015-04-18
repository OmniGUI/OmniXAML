namespace OmniXaml.Typing
{
    public interface IXamlNamespaceRegistry
    {
        //void RegisterPrefix(PrefixRegistration prefixRegistration);
        //XamlNamespace GetXamlNamespace(string ns);
        //void MapNamespaceTo(XamlNamespace xamlNamespace, ClrAssemblyPair targetAssembly);
        //XamlNamespace GetNamespaceForType(string typeName);
        //XamlNamespace GetNamespaceForType(Type type);
        //XamlNamespace GetNamespaceForType(XamlTypeName xamlTypeName);
        XamlNamespace GetXamlNamespace(string ns);
        string GetNamespaceForPrefix(string prefix);
        void RegisterPrefix(PrefixRegistration prefixRegistration);
        void RegisterNamespace(XamlNamespace xamlNamespace);
    }
}