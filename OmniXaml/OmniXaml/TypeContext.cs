namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Builder;
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

        public Namespace GetNamespace(string name)
        {
            return nsRegistry.GetNamespace(name);
        }

        public Namespace GetNamespaceByPrefix(string prefix)
        {
            return nsRegistry.GetNamespaceByPrefix(prefix);
        }

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            nsRegistry.RegisterPrefix(prefixRegistration);
        }

        public void AddNamespace(XamlNamespace xamlNamespace)
        {
            nsRegistry.AddNamespace(xamlNamespace);
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

        public XamlMember GetMember(PropertyInfo propertyInfo)
        {
            return typeRepository.GetMember(propertyInfo);
        }

        public ITypeFactory TypeFactory => typeFactory;

        public IEnumerable<PrefixRegistration> RegisteredPrefixes
        {
            get { return this.nsRegistry.RegisteredPrefixes; }
        }
    }
}