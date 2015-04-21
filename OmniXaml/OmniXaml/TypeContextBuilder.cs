namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typing;

    public class TypeContextBuilder
    {
        private IXamlTypeRepository typeRepository;
        private IXamlNamespaceRegistry nsRegistry;
        private readonly ITypeFactory typeFactory = new DefaultTypeFactory();

        private readonly ISet<ClrMapping> xamlMappings = new HashSet<ClrMapping>();
        private readonly IDictionary<string,string> prefixes = new Dictionary<string, string>();        

        public TypeContextBuilder()
        {
            nsRegistry = new XamlNamespaceRegistry();
            typeRepository = new XamlTypeRepository(nsRegistry);
        }

        public ITypeContext Build()
        {
            nsRegistry = new XamlNamespaceRegistry();
            typeRepository = new XamlTypeRepository(nsRegistry);

            RegisterPrefixes(nsRegistry);
            RegisterNamespaces(nsRegistry);

            return new TypeContext(typeRepository, nsRegistry, typeFactory);
        }

        private void RegisterPrefixes(IXamlNamespaceRegistry namespaceRegistry)
        {
            foreach (var prefix in prefixes)
            {
                namespaceRegistry.RegisterPrefix(new PrefixRegistration(prefix.Key, prefix.Value));
            }
        }

        private void RegisterNamespaces(IXamlNamespaceRegistry namespaceRegistry)
        {
            foreach (var xamlMapping in xamlMappings.ToLookup(mapping => mapping.XamlNamespace))
            {
                var xamlNs = xamlMapping.Key;
                var mappedTo = xamlMapping.Select(mapping => new ClrAssemblyPair(mapping.Assembly, mapping.ClrNamespace));

                namespaceRegistry.RegisterNamespace(new XamlNamespace(xamlNs, mappedTo));
            }
        }

        public TypeContextBuilder WithNsPrefix(string prefix, string ns)
        {
            prefixes.Add(prefix, ns);
            return this;
        }

        public TypeContextBuilder AddNsForThisType(string prefix, string xamlNs, Type referenceType)
        {
            WithNsPrefix(prefix, xamlNs);
            WithXamlNs(xamlNs, referenceType.GetTypeInfo().Assembly, referenceType.Namespace);
            return this;
        }

        public TypeContextBuilder WithXamlNs(string xamlNs, Assembly assembly, string clrNs)
        {
            xamlMappings.Add(new ClrMapping(assembly, xamlNs, clrNs));
            return this;
        }
    }
}