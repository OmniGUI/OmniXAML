namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System;
    using System.Reflection;
    using DefaultLoader;
    using OmniXaml.Ambient;

    public class ObjectBuilderTestsBase
    {
        protected static CreationFixture Create(ConstructionNode node, object rootInstance)
        {
            return CreateWithParams(node, (builder, ctNode, context) => builder.Inflate(ctNode, rootInstance, context));
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

            var creationContext = new BuildContext(
                new NamescopeAnnotator(constructionContext.MetadataProvider),
                new AmbientRegistrator(),
                new InstanceLifecycleSignaler());

            var executingAssembly = new[] { Assembly.GetExecutingAssembly() };

            var attributeBasedTypeDirectory = new AttributeBasedTypeDirectory(executingAssembly);
            var builder =
                new ExtendedObjectBuilder(
                    new InstanceCreator(constructionContext.SourceValueConverter, constructionContext, attributeBasedTypeDirectory),
                    constructionContext,
                    new ContextFactory(attributeBasedTypeDirectory, constructionContext));

            return new CreationFixture
            {
                Result = createFunc(builder, node, creationContext),
                BuildContext = creationContext
            };
        }
    }
}