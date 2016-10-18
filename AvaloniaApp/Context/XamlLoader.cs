namespace AvaloniaApp.Context
{
    using Avalonia.Controls;
    using OmniXaml;
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
            
            var type = typeof(Window);
            var ass = type.Assembly;
            var configuredAssemblyWithNamespaces = Route
                .Assembly(ass)
                .WithNamespaces("Avalonia.Controls");
            var xamlNamespace = XamlNamespace
                .Map("root")
                .With(configuredAssemblyWithNamespaces);
            typeDirectory.AddNamespace(xamlNamespace);

            directory.RegisterPrefix(new PrefixRegistration(string.Empty, "root"));

            return typeDirectory;
        }

        public object Load(string xaml)
        {
            
            var objectBuilder = new ExtendedObjectBuilder(new InstanceCreator(), Registrator.GetSourceValueConverter(), metadataProvider);
            var cons = GetConstructionNode(xaml);
            return objectBuilder.Create(cons);
        }

        private ConstructionNode GetConstructionNode(string xaml)
        {
            var sut = new XamlToTreeParser(directory, metadataProvider);
            var tree = sut.Parse(xaml);
            return tree;
        }
    }
}