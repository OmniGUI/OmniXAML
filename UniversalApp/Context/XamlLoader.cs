namespace Yuniversal.Context
{
    using System.Reflection;
    using Windows.UI.Xaml.Controls;
    using Adapters;
    using OmniXaml;
    using OmniXaml.Ambient;
    using OmniXaml.TypeLocation;

    public class XamlLoader
    {
        private readonly MetadataProvider metadataProvider;
        private readonly ITypeDirectory directory;

        public XamlLoader()
        {
            metadataProvider = new MetadataProvider();
            directory = RegisterTypeLocation();
        }

        private ITypeDirectory RegisterTypeLocation()
        {
            var type = typeof(Page);
            var ass = type.GetTypeInfo().Assembly;
            var xamlNamespace = XamlNamespace
                .Map("root")
                .With(
                    Route
                        .Assembly(ass)
                        .WithNamespaces(type.Namespace),
                    Route
                        .Assembly(typeof(OmniDataTemplate).GetTypeInfo().Assembly)
                        .WithNamespaces(typeof(OmniDataTemplate).Namespace));

            return new TypeDirectory(new[] { xamlNamespace });
        }

        public object Load(string xaml)
        {
            var constructionContext = new ObjectBuilderContext(
                Registrator.GetSourceValueConverter(),
                metadataProvider);

            var objectBuilder = new ExtendedObjectBuilder(new InstanceCreator(constructionContext.SourceValueConverter, constructionContext, directory), constructionContext, new ContextFactory(directory, constructionContext));

            var cons = GetConstructionNode(xaml);
            var prefixedTypeResolver = new PrefixedTypeResolver(new PrefixAnnotator(), directory);
            var buildContext = new BuildContext(new NamescopeAnnotator(metadataProvider), new AmbientRegistrator(), new InstanceLifecycleSignaler()) { PrefixedTypeResolver = prefixedTypeResolver};
            return objectBuilder.Inflate(cons.Root, buildContext);
        }

        private ParseResult GetConstructionNode(string xaml)
        {
            var resolver = new Resolver(directory);
            var sut = new XamlToTreeParser(metadataProvider, new[] { new InlineParser(resolver), }, resolver);
            var tree = sut.Parse(xaml, new PrefixAnnotator());
            return tree;
        }
    }
}