namespace WpfApplication1.Context
{
    using System.Windows;
    using OmniXaml;
    using OmniXaml.Ambient;
    using OmniXaml.TypeLocation;

    public class XamlLoader : IXamlLoader
    {
        private readonly TypeDirectory directory;
        private readonly ObjectBuilderContext objectBuilderContext;

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

            objectBuilderContext = new ObjectBuilderContext(
               new InstanceCreator(),
               Registrator.GetSourceValueConverter(),
               metadataProvider);
        }

        public ConstructionResult Load(string xaml)
        {
            var cn = GetConstructionNode(xaml);
            var objectBuilder = new ExtendedObjectBuilder(objectBuilderContext, (assignment, context, tc) => new ConverterValueContext(assignment, context, directory, tc), (assignment, context, tc) => new ValueContext(assignment, context, directory, tc));
            var namescopeAnnotator = new NamescopeAnnotator(objectBuilderContext.MetadataProvider);
            var instance = objectBuilder.Create(cn, new BuildContext(namescopeAnnotator, new AmbientRegistrator(), new InstanceLifecycleSignaler()));
            return new ConstructionResult(instance, namescopeAnnotator);
        }


        public ConstructionResult Load(string xaml, object intance)
        {
            var cn = GetConstructionNode(xaml);
            var objectBuilder = new ExtendedObjectBuilder(objectBuilderContext, (assignment, context, tc) => new ConverterValueContext(assignment, context, directory, tc), (assignment, context, tc) => new ValueContext(assignment, context, directory, tc));
            var namescopeAnnotator = new NamescopeAnnotator(objectBuilderContext.MetadataProvider);
            var instance = objectBuilder.Create(cn, intance, new BuildContext(namescopeAnnotator, new AmbientRegistrator(), new InstanceLifecycleSignaler()));
            return new ConstructionResult(instance, namescopeAnnotator);
        }

        private ConstructionNode GetConstructionNode(string xaml)
        {
            var parser = new XamlToTreeParser(directory, new MetadataProvider(), new[] { new InlineParser(directory), });
            var tree = parser.Parse(xaml);
            return tree;
        }
    }
}