namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System;
    using System.Reflection;
    using DefaultLoader;
    using Model;
    using Model.Custom;
    using Namespaces;
    using OmniXaml.Ambient;
    using TypeLocation;

    public class ObjectBuilderTestsBase
    {
        protected static CreationFixture Create(ConstructionNode node, object rootInstance)
        {
            return CreateWithParams(node, (builder, ctNode, context) => builder.Inflate(ctNode, context, rootInstance));
        }

        protected static CreationFixture Create(ConstructionNode node)
        {
            return CreateWithParams(node, (builder, ctNode, context) => builder.Inflate(ctNode, context));
        }

        private static CreationFixture CreateWithParams(ConstructionNode node, Func<IObjectBuilder, ConstructionNode, BuildContext, object> createFunc)
        {
            var constructionContext = new ObjectBuilderContext(
                new SourceValueConverter(),
                new AttributeBasedMetadataProvider());

            var prefixAnnotator = new PrefixAnnotator();


            var executingAssembly = new[] { Assembly.GetExecutingAssembly() };

            var attributeBasedTypeDirectory = new AttributeBasedTypeDirectory(executingAssembly);
            var builder =
                new ExtendedObjectBuilder(
                    new InstanceCreator(constructionContext.SourceValueConverter, constructionContext, attributeBasedTypeDirectory),
                    constructionContext,
                    new ContextFactory(attributeBasedTypeDirectory, constructionContext));


            var newTypeDirectory = GetTypeDirectory();

            var creationContext = new BuildContext(
               new NamescopeAnnotator(constructionContext.MetadataProvider),
               new AmbientRegistrator(),
               new InstanceLifecycleSignaler())
            { PrefixAnnotator = prefixAnnotator, PrefixedTypeResolver = new PrefixedTypeResolver(prefixAnnotator, newTypeDirectory) };

            return new CreationFixture
            {
                Result = createFunc(builder, node, creationContext),
                BuildContext = creationContext
            };
        }

        private static TypeDirectory GetTypeDirectory()
        {
            var type = typeof(TextBlock);
            var typeAnother = typeof(CustomControl);

            var assembly = type.GetTypeInfo().Assembly;
            var nsAnother = XamlNamespace.Map("another").With(Route.Assembly(assembly).WithNamespaces(typeAnother.Namespace));

            var newTypeDirectory = new TypeDirectory(new[] { nsAnother });
            return newTypeDirectory;
        }

    }
}