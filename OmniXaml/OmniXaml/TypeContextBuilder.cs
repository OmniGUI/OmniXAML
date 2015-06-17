namespace OmniXaml
{
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

        public TypeContextBuilder WithNamespaces(IEnumerable<XamlNamespace> namespaceRegistrations)
        {
            this.namespaceRegistrations = namespaceRegistrations;
            return this;
        }

        public TypeContextBuilder WithNsPrefixes(IEnumerable<PrefixRegistration> prefixRegistrations)
        {
            this.prefixRegistrations = prefixRegistrations;
            return this;
        }
    }
}