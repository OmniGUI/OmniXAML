namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Builder;
    using Catalogs;
    using Typing;

    public class TypeContextBuilder
    {
        private IXamlTypeRepository typeRepository;
        private IXamlNamespaceRegistry nsRegistry;
        private readonly ITypeFactory typeFactory = new DefaultTypeFactory();

        private readonly IDictionary<string,string> prefixes = new Dictionary<string, string>();
        private IEnumerable<XamlNamespace> namespaceRegistrations = new Collection<XamlNamespace>();
        private IEnumerable<PrefixRegistration> prefixRegistrations = new Collection<PrefixRegistration>();

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
            foreach (var prefix in prefixRegistrations)
            {
                namespaceRegistry.RegisterPrefix(prefix);
            }
        }

        private void RegisterNamespaces(IXamlNamespaceRegistry namespaceRegistry)
        {
            foreach (var xamlNamespace in namespaceRegistrations)
            {               
                namespaceRegistry.AddNamespace(xamlNamespace);
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
            return this;
        }

        public void WithNamespaces(IEnumerable<XamlNamespace> namespaceRegistrations)
        {
            this.namespaceRegistrations = namespaceRegistrations;
        }

        public void WithNsPrefixes(IEnumerable<PrefixRegistration> prefixRegistrations)
        {
            this.prefixRegistrations = prefixRegistrations;
        }
    }
}