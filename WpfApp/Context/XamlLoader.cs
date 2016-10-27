namespace WpfApplication1.Context
{
    using System.Windows;
    using OmniXaml;
    using OmniXaml.TypeLocation;

    public class XamlLoader : IXamlLoader
    {
        private readonly TypeDirectory directory;
        private readonly ConstructionContext constructionContext;

        public XamlLoader()
        {
            var metadataProvider = new MetadataProvider();
            
            var type = typeof(Window);
            var ass = type.Assembly;

            directory = new TypeDirectory();
            directory.RegisterPrefix(new PrefixRegistration(string.Empty, "root"));

            var configuredAssemblyWithNamespaces = Route
                .Assembly(ass)
                .WithNamespaces("System.Windows", "System.Windows.Controls");
            var xamlNamespace = XamlNamespace
                .Map("root")
                .With(configuredAssemblyWithNamespaces);
            directory.AddNamespace(xamlNamespace);

            constructionContext = new ConstructionContext(
               new InstanceCreator(),
               Registrator.GetSourceValueConverter(),
               metadataProvider,
               new InstanceLifecycleSignaler());
        }

        public object Load(string xaml)
        {
            var cn = GetConstructionNode(xaml);
            var objectBuilder = new ExtendedObjectBuilder(constructionContext, (assignment, context) => new MarkupExtensionContext(assignment, context, directory));
            return objectBuilder.Create(cn, new NamescopeAnnotator());
        }


        public object Load(string xaml, object intance)
        {
            var cn = GetConstructionNode(xaml);
            var objectBuilder = new ExtendedObjectBuilder(constructionContext, (assignment, context) => new MarkupExtensionContext(assignment, context, directory));
            return objectBuilder.Create(cn, new NamescopeAnnotator());
        }

        private ConstructionNode GetConstructionNode(string xaml)
        {
            var parser = new XamlToTreeParser(directory, new MetadataProvider(), new[] { new InlineParser(directory), });
            var tree = parser.Parse(xaml);
            return tree;
        }
    }
}