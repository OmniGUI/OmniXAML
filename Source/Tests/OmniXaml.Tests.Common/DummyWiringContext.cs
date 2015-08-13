namespace OmniXaml.Tests.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Builder;
    using Classes;
    using Classes.Another;
    using Classes.WpfLikeModel;
    using Glass;
    using TypeConversion;
    using Typing;

    public class DummyWiringContext : IWiringContext
    {
        private readonly IWiringContext wiringContext;

        public DummyWiringContext(ITypeFactory factory, IEnumerable<Assembly> assembliesToScan)
        {
            var assemblies = assembliesToScan.ToList();
            var featureProvider = GetFeatureProvider(assemblies);
            var typeContext = GetTypeContext(factory, featureProvider);
            wiringContext = new WiringContext(typeContext, featureProvider);
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

        private static ITypeContext GetTypeContext(ITypeFactory typeFactory, ITypeFeatureProvider featureProvider)
        {
            var xamlNamespaceRegistry = CreateXamlNamespaceRegistry();            
            return new TypeContext(new XamlTypeRepository(xamlNamespaceRegistry, typeFactory, featureProvider), xamlNamespaceRegistry, typeFactory);
        }

        public ITypeContext TypeContext => wiringContext.TypeContext;
        public ITypeFeatureProvider FeatureProvider => wiringContext.FeatureProvider;
    }
}