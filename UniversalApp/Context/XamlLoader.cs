namespace Yuniversal.Context
{
    using System.Reflection;
    using Windows.UI.Xaml.Controls;
    using Adapters;
    using OmniXaml;
    using OmniXaml.Tests;
    using OmniXaml.TypeLocation;

    public class XamlLoader
    {
        private readonly MetadataProvider metadataProvider;
        private readonly TypeDirectory directory;

        public XamlLoader()
        {
            metadataProvider = new MetadataProvider();
            directory = RegisterTypeLocation();
        }

        private TypeDirectory RegisterTypeLocation()
        {
            var typeDirectory = new TypeDirectory();

            var type = typeof(Page);
            var ass = type.GetTypeInfo().Assembly;
            typeDirectory.AddNamespace(
                XamlNamespace
                    .Map("root")
                    .With(
                        Route
                            .Assembly(ass)
                            .WithNamespaces(type.Namespace),
                        Route
                            .Assembly(typeof(OmniDataTemplate).GetTypeInfo().Assembly)
                            .WithNamespaces(typeof(OmniDataTemplate).Namespace))
            );

            typeDirectory.RegisterPrefix(new PrefixRegistration(string.Empty, "root"));

            return typeDirectory;
        }

        public object Load(string xaml)
        {
            var constructionContext = new ObjectBuilderContext(
                Registrator.GetSourceValueConverter(),
                metadataProvider);

            var objectBuilder = new ExtendedObjectBuilder(new InstanceCreator(constructionContext.SourceValueConverter, constructionContext, directory), constructionContext, new ContextFactory(directory, constructionContext));

            var cons = GetConstructionNode(xaml);
            return objectBuilder.Inflate(cons, new BuildContext(new NamescopeAnnotator(metadataProvider), null, new InstanceLifecycleSignaler()));
        }

        private ConstructionNode GetConstructionNode(string xaml)
        {
            var sut = new XamlToTreeParser(directory, metadataProvider, new []{new InlineParser(directory), });
            var tree = sut.Parse(xaml);
            return tree;
        }
    }
}