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
            objectBuilderContext = new ObjectBuilderContext(new SourceValueConverter(), metadataProvider);
        }

        public ConstructionResult Load(string xaml, object intance = null)
        {
            var ct = Parse(xaml);
            return Construct(ct.Root, intance);
        }

        private ConstructionResult Construct(ConstructionNode ctNode, object intance)
        {
            var namescopeAnnotator = new NamescopeAnnotator(metadataProvider);
            var trackingContext = new BuildContext(namescopeAnnotator, new AmbientRegistrator(), new InstanceLifecycleSignaler());
            var instanceCreator = new InstanceCreator(objectBuilderContext.SourceValueConverter, objectBuilderContext, directory);
            var objectConstructor = new ObjectBuilder(instanceCreator, objectBuilderContext, new ContextFactory(directory, objectBuilderContext));
            var construct = objectConstructor.Inflate(ctNode, trackingContext, intance);
            return new ConstructionResult(construct, namescopeAnnotator);
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