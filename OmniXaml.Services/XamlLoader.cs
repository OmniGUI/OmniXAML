namespace OmniXaml.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Ambient;
    using TypeLocation;

    public class XamlLoader : IXamlLoader
    {
        private readonly ObjectBuilderContext objectBuilderContext;
        private readonly ITypeDirectory directory;
        private readonly AttributeBasedMetadataProvider metadataProvider;

        public XamlLoader(IList<Assembly> assemblies)
        {
            directory = new AttributeBasedTypeDirectory(assemblies);
            metadataProvider = new AttributeBasedMetadataProvider();
            objectBuilderContext = new ObjectBuilderContext(new AttributeBasedSourceValueConverter(assemblies), metadataProvider);
        }

        public ConstructionResult Load(string xaml, object intance = null)
        {
            var ct = Parse(xaml);
            return Construct(ct.Root, intance);
        }

        private ConstructionResult Construct(ConstructionNode ctNode, object intance)
        {
            var namescopeAnnotator = new NamescopeAnnotator(metadataProvider);
            var prefixedTypeResolver = new PrefixedTypeResolver(new PrefixAnnotator(), directory);

            var trackingContext = new BuildContext(namescopeAnnotator, new AmbientRegistrator(), new InstanceLifecycleSignaler()) { PrefixedTypeResolver = prefixedTypeResolver};
            var instanceCreator = GetInstanceCreator(objectBuilderContext.SourceValueConverter, objectBuilderContext, directory);
            var objectConstructor = GetObjectBuilder(instanceCreator, objectBuilderContext, new ContextFactory(directory, objectBuilderContext));
            var construct = objectConstructor.Inflate(ctNode, trackingContext, intance);
            return new ConstructionResult(construct, namescopeAnnotator);
        }

        protected virtual IInstanceCreator GetInstanceCreator(ISourceValueConverter sourceValueConverter, ObjectBuilderContext context, ITypeDirectory typeDirectory)
        {
            return new InstanceCreator(sourceValueConverter, context, typeDirectory);
        }

        protected virtual IObjectBuilder GetObjectBuilder(IInstanceCreator instanceCreator, ObjectBuilderContext context, ContextFactory factory)
        {
            return new ExtendedObjectBuilder(instanceCreator, context, factory);
        }

        private ParseResult Parse(string xaml)
        {
            var resolver = new Resolver(directory);
            var sut = new XamlToTreeParser(metadataProvider, new[] {new InlineParser(resolver) }, resolver);

            var tree = sut.Parse(xaml, new PrefixAnnotator());
            return tree;
        }
    }
}