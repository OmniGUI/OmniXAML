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

    public class TestRuntimeTypeSource : IRuntimeTypeSource
    {
        private readonly IRuntimeTypeSource inner;
        private readonly TestTypeRepository testTypeRepository;

        public TestRuntimeTypeSource()
        {
            var namespaceRegistry = CreateNamespaceRegistry();

            var featureProvider = new TypeFeatureProvider(new TypeConverterProvider());
            featureProvider.FillFromAttributes(ScannedAssemblies.AllExportedTypes());
            testTypeRepository = new TestTypeRepository(namespaceRegistry, new TypeFactory(), featureProvider);
            
            inner = new RuntimeTypeSource(testTypeRepository, namespaceRegistry);
            inner.RegisterPrefix(new PrefixRegistration("", "root"));
            inner.RegisterPrefix(new PrefixRegistration("x", "another"));
        }

        private INamespaceRegistry CreateNamespaceRegistry()
        {
            var namespaceRegistry = new NamespaceRegistry();
            namespaceRegistry.FillFromAttributes(ScannedAssemblies);

            return namespaceRegistry;
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
        public XamlType GetByType(Type type)
        {
            return inner.GetByType(type);
        }

        public XamlType GetByQualifiedName(string qualifiedName)
        {
            return inner.GetByQualifiedName(qualifiedName);
        }

        public XamlType GetByPrefix(string prefix, string typeName)
        {
            return inner.GetByPrefix(prefix, typeName);
        }

        public XamlType GetByFullAddress(XamlTypeName xamlTypeName)
        {
            return inner.GetByFullAddress(xamlTypeName);
        }

        public Member GetMember(PropertyInfo propertyInfo)
        {
            return inner.GetMember(propertyInfo);
        }

        public AttachableMember GetAttachableMember(string name, MethodInfo getter, MethodInfo setter)
        {
            return inner.GetAttachableMember(name, getter, setter);
        }

        public void EnableNameScope<T>()
        {
            testTypeRepository.EnableNameScope(typeof(T));
        }

        public void ClearNamescopes()
        {
            testTypeRepository.ClearNameScopes();
        }
    }
}