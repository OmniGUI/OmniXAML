namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Builder;
    using TypeConversion;
    using Typing;

    public class TypeContextBuilder
    {
        private IXamlTypeRepository typeRepository;
        private IXamlNamespaceRegistry nsRegistry;
        private ITypeFactory typeFactory = new TypeFactory();
        private ITypeFeatureProvider featureProvider = new TypeFeatureProvider(new ContentPropertyProvider(), new TypeConverterProvider());

        private IEnumerable<XamlNamespace> namespaceRegistrations = new Collection<XamlNamespace>();
        private IEnumerable<PrefixRegistration> prefixRegistrations = new Collection<PrefixRegistration>();

        public TypeContextBuilder()
        {
            nsRegistry = new XamlNamespaceRegistry();
            typeRepository = new XamlTypeRepository(nsRegistry, typeFactory, featureProvider);
        }

        public ITypeContext Build()
        {
            nsRegistry = new XamlNamespaceRegistry();
            
            typeRepository = new XamlTypeRepository(nsRegistry, typeFactory, featureProvider);

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

        public TypeContextBuilder WithTypeFactory(ITypeFactory typeFactory)
        {
            this.typeFactory = typeFactory;
            return this;
        }
    }
}