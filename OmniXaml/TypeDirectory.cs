namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Glass.Core;
    using TypeLocation;

    public class TypeDirectory : ITypeDirectory
    {
        private const string ClrNamespace = "clr-namespace:";
        private readonly IDictionary<string, ClrNamespace> clrNamespaces = new Dictionary<string, ClrNamespace>();
        private readonly ICollection<PrefixRegistration> prefixRegistrations = new Collection<PrefixRegistration>();
        private readonly IDictionary<string, string> registeredPrefixes = new Dictionary<string, string>();
        private readonly ISet<XamlNamespace> xamlNamespaces = new HashSet<XamlNamespace>();

        public IEnumerable<PrefixRegistration> RegisteredPrefixes => prefixRegistrations;

        public Namespace GetNamespaceByPrefix(string prefix)
        {
            ClrNamespace clrNs;
            var isClrNs = clrNamespaces.TryGetValue(prefix, out clrNs);
            if (isClrNs)
                return clrNs;

            var namespaceName = GetXamlNamespaceNameByPrefix(prefix);

            return GetXamlNamespace(namespaceName);
        }

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            if (registeredPrefixes.ContainsKey(prefixRegistration.Prefix))
            {
                return;
            }

            var prefix = prefixRegistration.Prefix;
            var ns = prefixRegistration.Ns;

            RegisterWhenItsClrNs(prefix, ns);

            prefixRegistrations.Add(prefixRegistration);
            registeredPrefixes.Add(prefixRegistration.Prefix, ns);
        }

        public XamlNamespace GetXamlNamespace(string ns)
        {
            var xamlNamespace = xamlNamespaces.FirstOrDefault(ns1 => ns1.Name == ns);
            return xamlNamespace;
        }

        public Namespace GetNamespace(string name)
        {
            return xamlNamespaces.FirstOrDefault(xamlNamespace => xamlNamespace.Name == name);
        }

        public XamlNamespace GetXamlNamespaceByPrefix(string prefix)
        {
            return GetXamlNamespace(registeredPrefixes[prefix]);
        }

        public void AddNamespace(XamlNamespace xamlNamespace)
        {
            xamlNamespaces.Add(xamlNamespace);
        }

        public ClrNamespace GetClrNamespaceByPrefix(string prefix)
        {
            return clrNamespaces[prefix];
        }

        public Type GetTypeByPrefix(string prefix, string typeName)
        {
            var ns = GetNamespaceByPrefix(prefix);

            if (ns == null)
                throw new Exception($"Cannot find a namespace with the prefix \"{prefix}\"");

            var type = ns.Get(typeName);

            if (type == null)
                throw new Exception($"The type \"{{{prefix}:{typeName}}} cannot be found\"");

            return type;
        }

        public Type GetTypeByFullAddres(Address address)
        {
            var ns = GetNamespace(address.Namespace);

            if (ns == null)
                throw new Exception($"Error trying to resolve a XamlType: Cannot find the namespace '{address.Namespace}'");

            var correspondingType = ns.Get(address.TypeName);

            if (correspondingType == null)
                throw new Exception(
                    $"Error trying to resolve a XamlType: The type {address.TypeName} has not been found into the namespace '{address.Namespace}'");

            return correspondingType;
        }

        public Type GetByPrefixedName(string prefixedName)
        {
            var tuple = prefixedName.Dicotomize(':');

            var prefix = tuple.Item2 == null ? string.Empty : tuple.Item1;
            var typeName = tuple.Item2 ?? tuple.Item1;

            return GetTypeByPrefix(prefix, typeName);
        }

        private string GetXamlNamespaceNameByPrefix(string prefix)
        {
            bool isFound;
            string name;
            isFound = registeredPrefixes.TryGetValue(prefix, out name);

            if (!isFound)
                throw new KeyNotFoundException($"The prefix {prefix} is not registered");

            return name;
        }

        private void RegisterWhenItsClrNs(string prefix, string ns)
        {
            if (IsClrNamespace(ns))
                clrNamespaces.Add(prefix, ExtractNamespace(ns));
        }

        private static ClrNamespace ExtractNamespace(string formattedClrString)
        {
            var startOfNamespace = formattedClrString.IndexOf(":", StringComparison.Ordinal) + 1;
            var endOfNamespace = formattedClrString.IndexOf(";", startOfNamespace, StringComparison.Ordinal);

            if (endOfNamespace < 0)
                endOfNamespace = formattedClrString.Length - startOfNamespace;

            var ns = formattedClrString.Substring(startOfNamespace, endOfNamespace - startOfNamespace);

            var remainingPartStart = startOfNamespace + ns.Length + 1;
            var remainingPartLenght = formattedClrString.Length - remainingPartStart;
            var assemblyPart = formattedClrString.Substring(remainingPartStart, remainingPartLenght);

            var assembly = GetAssembly(assemblyPart);

            return new ClrNamespace(assembly, ns);
        }

        private static Assembly GetAssembly(string assemblyPart)
        {
            var dicotomize = assemblyPart.Dicotomize('=');
            return Assembly.Load(new AssemblyName(dicotomize.Item2));
        }

        private static bool IsClrNamespace(string ns)
        {
            return ns.StartsWith(ClrNamespace);
        }
    }
}