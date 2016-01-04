namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Builder;
    using Glass;
    using Typing;

    public class TypeContext : ITypeContext
    {
        public IXamlNamespaceRegistry NamespaceRegistry { get; }

        public TypeContext(IXamlTypeRepository typeRepository, IXamlNamespaceRegistry nsRegistry)
        {
            TypeRepository = typeRepository;
            NamespaceRegistry = nsRegistry;
        }

        public Namespace GetNamespace(string name)
        {
            return NamespaceRegistry.GetNamespace(name);
        }

        public Namespace GetNamespaceByPrefix(string prefix)
        {
            return NamespaceRegistry.GetNamespaceByPrefix(prefix);
        }

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            NamespaceRegistry.RegisterPrefix(prefixRegistration);
        }

        public void AddNamespace(XamlNamespace xamlNamespace)
        {
            NamespaceRegistry.AddNamespace(xamlNamespace);
        }

        public XamlType GetXamlType(Type type)
        {
            return TypeRepository.GetXamlType(type);
        }

        public XamlType GetByQualifiedName(string qualifiedName)
        {

            return TypeRepository.GetByQualifiedName(qualifiedName);
        }

        public XamlType GetByPrefix(string prefix, string typeName)
        {
            return TypeRepository.GetByPrefix(prefix, typeName);
        }

        public XamlType GetWithFullAddress(XamlTypeName xamlTypeName)
        {
            return TypeRepository.GetWithFullAddress(xamlTypeName);
        }

        public XamlMember GetMember(PropertyInfo propertyInfo)
        {
            return TypeRepository.GetMember(propertyInfo);
        }

        public AttachableXamlMember GetAttachableMember(string name, MethodInfo getter, MethodInfo setter)
        {
            return TypeRepository.GetAttachableMember(name, getter, setter);
        }

        public Metadata GetMetadata(XamlType xamlType)
        {
            return TypeRepository.GetMetadata(xamlType);
        }

        public void RegisterMetadata(Type type, Metadata metadata)
        {
            TypeRepository.RegisterMetadata(type, metadata);
        }

        public IEnumerable<PrefixRegistration> RegisteredPrefixes => NamespaceRegistry.RegisteredPrefixes;

        public IXamlTypeRepository TypeRepository { get; }

        public static ITypeContext FromAttributes(IEnumerable<Assembly> assemblies)
        {
            var allExportedTypes = assemblies.AllExportedTypes();
            var registry = XamlNamespaceRegistry.FromAttributes(assemblies);
            var typeFactory = new TypeFactory();

            var featureProvider = new TypeFeatureProviderBuilder().FromAttributes(allExportedTypes).Build();
            var xamlTypeRepo = new XamlTypeRepository(registry, typeFactory, featureProvider);

            return new TypeContext(xamlTypeRepo, registry);
        }
    }
}