namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Builder;
    using Glass.Core;

    public class NamespaceRegistry : INamespaceRegistry
    {
        private const string ClrNamespace = "clr-namespace:";
        private readonly IDictionary<string, string> registeredPrefixes = new Dictionary<string, string>();
        private readonly ISet<XamlNamespace> xamlNamespaces = new HashSet<XamlNamespace>();
        private readonly IDictionary<string, ClrNamespace> clrNamespaces = new Dictionary<string, ClrNamespace>();
        private readonly ICollection<PrefixRegistration> prefixRegistrations = new Collection<PrefixRegistration>();

        public IEnumerable<PrefixRegistration> RegisteredPrefixes => prefixRegistrations;

        public Namespace GetNamespaceByPrefix(string prefix)
        {
            ClrNamespace clrNs;
            var isClrNs = clrNamespaces.TryGetValue(prefix, out clrNs);
            if (isClrNs)
            {
                return clrNs;
            }

            var namespaceName = GetXamlNamespaceNameByPrefix(prefix);

            return GetXamlNamespace(namespaceName);
        }

        private string GetXamlNamespaceNameByPrefix(string prefix)
        {
            bool isFound;
            string name;
            isFound = registeredPrefixes.TryGetValue(prefix, out name);

            if (!isFound)
            {
                throw new KeyNotFoundException($"The prefix {prefix} is not registered");
            }

            return name;
        }

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            if (!registeredPrefixes.ContainsKey(prefixRegistration.Prefix))
            {
                var prefix = prefixRegistration.Prefix;
                var @namespace = prefixRegistration.Ns;

                RegisterWhenItsClrNs(prefix, @namespace);

                prefixRegistrations.Add(prefixRegistration);
                registeredPrefixes.Add(prefixRegistration.Prefix, @namespace);
            }
        }

        private void RegisterWhenItsClrNs(string prefix, string @namespace)
        {
            if (IsClrNamespace(@namespace))
            {
                clrNamespaces.Add(prefix, ExtractNamespace(@namespace));
            }
        }

        private static ClrNamespace ExtractNamespace(string formattedClrString)
        {
            var startOfNamespace = formattedClrString.IndexOf(":", StringComparison.Ordinal) + 1;
            var endOfNamespace = formattedClrString.IndexOf(";", startOfNamespace, StringComparison.Ordinal);

            if (endOfNamespace < 0)
            {
                endOfNamespace = formattedClrString.Length - startOfNamespace;
            }

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
    }
}