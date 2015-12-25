namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Reflection;
    using Glass;
    using Typing;

    public class WiringContext : IWiringContext
    {
        public WiringContext(ITypeContext typeContext, ITypeFeatureProvider typeFeatureProvider)
        {
            FeatureProvider = typeFeatureProvider;
            TypeContext = typeContext;            
        }

        public ITypeContext TypeContext { get; }

        public ITypeFeatureProvider FeatureProvider { get; }

        public static IWiringContext FromAttributes(IEnumerable<Assembly> assemblies)
        {
            var allExportedTypes = assemblies.AllExportedTypes();
            var registry = XamlNamespaceRegistry.FromAttributes(assemblies);
            var typeFactory = new TypeFactory();

            var featureProvider = new TypeFeatureProviderBuilder().FromAttributes(allExportedTypes).Build();
            var xamlTypeRepo = new XamlTypeRepository(registry, typeFactory, featureProvider);
            var typeContext = new TypeContext(xamlTypeRepo, registry, typeFactory);

            return new WiringContext(typeContext, featureProvider);
        }
    }
}