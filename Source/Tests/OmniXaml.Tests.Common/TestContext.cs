namespace OmniXaml.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Builder;
    using Classes;
    using Glass;
    using TypeConversion;
    using Typing;

    public class TestContext : ITypeContext
    {
        private readonly ITypeContext inner;
        private readonly TestXamlTypeRepository testXamlTypeRepository;

        public TestContext()
        {
            var xamlNamespaceRegistry = CreateXamlNamespaceRegistry();

            ITypeFeatureProvider featureProvider = new TypeFeatureProvider(new TypeConverterProvider());
            featureProvider.FillFromAttributes(ScannedAssemblies.AllExportedTypes());
            testXamlTypeRepository = new TestXamlTypeRepository(xamlNamespaceRegistry, new TypeFactory(), featureProvider);
            
            inner = new TypeContext(testXamlTypeRepository, xamlNamespaceRegistry);
        }

        private IXamlNamespaceRegistry CreateXamlNamespaceRegistry()
        {
            var xamlNamespaceRegistry = new XamlNamespaceRegistry();
            xamlNamespaceRegistry.FillFromAttributes(ScannedAssemblies);

            return xamlNamespaceRegistry;
        }

        private static IEnumerable<Assembly> ScannedAssemblies => new List<Assembly> { typeof(DummyClass).GetTypeInfo().Assembly };


        public Namespace GetNamespace(string name)
        {
            return inner.GetNamespace(name);
        }

        public Namespace GetNamespaceByPrefix(string prefix)
        {
            return inner.GetNamespaceByPrefix(prefix);
        }

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            inner.RegisterPrefix(prefixRegistration);
        }

        public void AddNamespace(XamlNamespace xamlNamespace)
        {
            inner.AddNamespace(xamlNamespace);
        }

        public IEnumerable<PrefixRegistration> RegisteredPrefixes => inner.RegisteredPrefixes;
        public XamlType GetXamlType(Type type)
        {
            return inner.GetXamlType(type);
        }

        public XamlType GetByQualifiedName(string qualifiedName)
        {
            return inner.GetByQualifiedName(qualifiedName);
        }

        public XamlType GetByPrefix(string prefix, string typeName)
        {
            return inner.GetByPrefix(prefix, typeName);
        }

        public XamlType GetWithFullAddress(XamlTypeName xamlTypeName)
        {
            return inner.GetWithFullAddress(xamlTypeName);
        }

        public XamlMember GetMember(PropertyInfo propertyInfo)
        {
            return inner.GetMember(propertyInfo);
        }

        public AttachableXamlMember GetAttachableMember(string name, MethodInfo getter, MethodInfo setter)
        {
            return inner.GetAttachableMember(name, getter, setter);
        }

        public void EnableNameScope<T>()
        {
            testXamlTypeRepository.EnableNameScope(typeof(T));
        }

        public void ClearNamescopes()
        {
            testXamlTypeRepository.ClearNameScopes();
        }
    }
}