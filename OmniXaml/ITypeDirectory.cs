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
        void AddNamespace(XamlNamespace xamlNamespace);
        Type GetTypeByPrefix(string prefix, string typeName);
        Type GetTypeByFullAddres(Address address);
    }
}