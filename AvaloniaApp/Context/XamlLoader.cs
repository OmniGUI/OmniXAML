namespace AvaloniaApp.Context
{
    using Adapters;
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
            typeDirectory.AddNamespace(
                    XamlNamespace
                        .Map("root")
                        .With(
                            Route
                                .Assembly(ass)
                                .WithNamespaces("Avalonia.Controls"),
                            Route
                                .Assembly(typeof(OmniDataTemplate).Assembly)
                                .WithNamespaces(typeof(OmniDataTemplate).Namespace)));
          

            typeDirectory.RegisterPrefix(new PrefixRegistration(string.Empty, "root"));

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