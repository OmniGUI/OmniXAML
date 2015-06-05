namespace OmniXaml
{
    using System;
    using Catalogs;
    using Typing;

    public class TypeContext : ITypeContext
    {
        private readonly IXamlTypeRepository typeRepository;
        private readonly IXamlNamespaceRegistry nsRegistry;
        private readonly ITypeFactory typeFactory;

        public TypeContext(IXamlTypeRepository typeRepository, IXamlNamespaceRegistry nsRegistry, ITypeFactory typeFactory)
        {
            this.typeRepository = typeRepository;
            this.nsRegistry = nsRegistry;
            this.typeFactory = typeFactory;
        }

        public XamlNamespace GetXamlNamespace(string ns)
        {
            return nsRegistry.GetXamlNamespace(ns);
        }

        public string GetNamespaceForPrefix(string prefix)
        {
            return nsRegistry.GetNamespaceForPrefix(prefix);
        }

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            nsRegistry.RegisterPrefix(prefixRegistration);
        }

        public void RegisterNamespace(XamlNamespace xamlNamespace)
        {
            nsRegistry.RegisterNamespace(xamlNamespace);
        }

        public void AddCatalog(AttributeBasedClrMappingCatalog attributeBasedClrMappingCatalog)
        {
            nsRegistry.AddCatalog(attributeBasedClrMappingCatalog);
        }

        public XamlType Get(Type type)
        {
            return typeRepository.Get(type);
        }

        public XamlType GetByPrefix(string prefix, string typeName)
        {
            return typeRepository.GetByPrefix(prefix, typeName);
        }

        public XamlType GetWithFullAddress(XamlTypeName xamlTypeName)
        {
            return typeRepository.GetWithFullAddress(xamlTypeName);
        }

        public ITypeFactory TypeFactory => typeFactory;
    }
}