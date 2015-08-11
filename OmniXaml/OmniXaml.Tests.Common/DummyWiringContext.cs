namespace OmniXaml.Tests.Common
{
    using System.Collections.Generic;
    using System.Reflection;
    using Builder;
    using Classes;
    using Classes.Another;
    using Classes.WpfLikeModel;
    using TypeConversion;
    using Typing;

    public class DummyWiringContext : WiringContext
    {
        public DummyWiringContext(ITypeFactory factory) : base(GetTypeContext(factory), GetFeatureProvider())
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

        private static ITypeFeatureProvider GetFeatureProvider()
        {
            var contentPropertyProvider = new ContentPropertyProvider();
            var contentProperties = ContentProperties.DefinedInAssemblies(new[] { typeof(DummyClass).GetTypeInfo().Assembly });
            foreach (var contentPropertyDefinition in contentProperties)
            {
                contentPropertyProvider.Add(contentPropertyDefinition);
            }

            return new TypeFeatureProvider(contentPropertyProvider, new TypeConverterProvider());
        }

        private static ITypeContext GetTypeContext(ITypeFactory typeFactory)
        {
            var xamlNamespaceRegistry = CreateXamlNamespaceRegistry();            
            return new TypeContext(new XamlTypeRepository(xamlNamespaceRegistry, typeFactory, GetFeatureProvider()), xamlNamespaceRegistry, typeFactory);
        }
    }
}