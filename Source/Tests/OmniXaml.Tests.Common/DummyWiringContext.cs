namespace OmniXaml.Tests.Common
{
    using System.Collections.Generic;
    using System.Reflection;
    using Builder;
    using Classes;
    using Classes.Another;
    using Classes.WpfLikeModel;
    using Glass;
    using Typing;

    public class DummyWiringContext : WiringContext
    {
        public DummyWiringContext(ITypeFactory factory, IEnumerable<Assembly> assembliesToScan)
            : this(factory, GetFeatureProvider(assembliesToScan), assembliesToScan)
        {
        }

        private DummyWiringContext(ITypeFactory factory, ITypeFeatureProvider typeFeatureProvider, IEnumerable<Assembly> assembliesToScan)
            : base(GetTypeContext(factory, typeFeatureProvider, assembliesToScan), typeFeatureProvider)
        {
        }

        private static XamlNamespaceRegistry CreateXamlNamespaceRegistry()
        {
            var xamlNamespaceRegistry = new XamlNamespaceRegistry();

            var rootType = typeof(DummyClass);
            var anotherType = typeof(Foreigner);

            var definitionForRoot = XamlNamespace
                .Map("root")
                .With(
                    new[]
                    {
                        Route.Assembly(rootType.GetTypeInfo().Assembly)
                            .WithNamespaces(
                                new[]
                                {
                                    rootType.Namespace,
                                    typeof (Window).Namespace,
                                })
                    });

            var definitionForAnother = XamlNamespace
                .Map("another")
                .With(
                    new[]
                    {
                        Route.Assembly(anotherType.GetTypeInfo().Assembly)
                            .WithNamespaces(new[] {anotherType.Namespace})
                    });

            foreach (var ns in new List<XamlNamespace> { definitionForRoot, definitionForAnother })
            {
                xamlNamespaceRegistry.AddNamespace(ns);
            }

            xamlNamespaceRegistry.RegisterPrefix(new PrefixRegistration("", "root"));
            xamlNamespaceRegistry.RegisterPrefix(new PrefixRegistration("x", "another"));

            return xamlNamespaceRegistry;
        }

        private static ITypeFeatureProvider GetFeatureProvider(IEnumerable<Assembly> assembliesToScan)
        {
            var builder = new TypeFeatureProviderBuilder().FromAttributes(assembliesToScan.AllExportedTypes());
            return builder.Build();
        }

        private static ITypeContext GetTypeContext(ITypeFactory typeFactory, ITypeFeatureProvider featureProvider, IEnumerable<Assembly> assembliesToScan)
        {
            var xamlNamespaceRegistry = CreateXamlNamespaceRegistry();

            var dummyXamlTypeRepository = new DummyXamlTypeRepository(xamlNamespaceRegistry, typeFactory, featureProvider);

            dummyXamlTypeRepository.RegisterMetadata(new Metadata<DummyObject>().WithRuntimeNameProperty(d => d.Name));
            dummyXamlTypeRepository.RegisterMetadata(new Metadata<Setter>().WithMemberDependency(setter => setter.Value, setter => setter.Property));
            dummyXamlTypeRepository.RegisterMetadata(new Metadata<ComboBox>().WithMemberDependency(setter => setter.SelectedIndex, setter => setter.Items));

            return new TypeContext(dummyXamlTypeRepository, xamlNamespaceRegistry, typeFactory);
        }

        private DummyXamlTypeRepository DummyXamlTypeRepository => (DummyXamlTypeRepository) TypeContext.TypeRepository;


        public void ClearNamesCopes()
        {
            DummyXamlTypeRepository.ClearNameScopes();
        }

        public void EnableNameScope<T>()
        {
            DummyXamlTypeRepository.EnableNameScope(typeof(T));
        }        
    }
}