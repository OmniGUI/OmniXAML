namespace OmniXaml.TypeLocation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TypeDirectory : ITypeDirectory
    {
        private const string ClrNamespace = "using:";
        private readonly ISet<XamlNamespace> xamlNamespaces;

        public TypeDirectory(IEnumerable<XamlNamespace> xamlNamespaces)
        {
            this.xamlNamespaces =  new HashSet<XamlNamespace>(xamlNamespaces);
        }

        public Type GetTypeByFullAddress(Address address)
        {
            var ns = GetNamespace(address.Namespace);

            if (ns == null)
            {
                throw new Exception($"Error trying to resolve a XamlType: Cannot find the namespace '{address.Namespace}'");                
            }

            return ns.Get(address.TypeName);
        }

        private Namespace GetNamespace(string name)
        {
            if (IsClrNamespace(name))
            {
                return TypeLocation.ClrNamespace.ExtractNamespace(name);
            }

            return xamlNamespaces.FirstOrDefault(xamlNamespace => xamlNamespace.Name == name);
        }

        private static bool IsClrNamespace(string ns)
        {
            return ns.StartsWith(ClrNamespace);
        }
    }
}