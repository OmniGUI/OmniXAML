namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using TypeLocation;

    public interface ITypeDirectory
    {
        IEnumerable<PrefixRegistration> RegisteredPrefixes { get; }
        Namespace GetNamespaceByPrefix(string prefix);
        void RegisterPrefix(PrefixRegistration prefixRegistration);
        XamlNamespace GetXamlNamespace(string namespaceName);
        Namespace GetNamespace(string name);
        XamlNamespace GetXamlNamespaceByPrefix(string prefix);
        void AddNamespace(XamlNamespace xamlNamespace);
        ClrNamespace GetClrNamespaceByPrefix(string prefix);
        Type GetTypeByPrefix(string prefix, string typeName);
        Type GetTypeByFullAddres(Address address);
        Type GetByPrefixedName(string prefixedName);
    }
}