namespace WpfApplication1.Context
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;
    using OmniXaml;
    using OmniXaml.Ambient;
    using OmniXaml.Tests;
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

            var sourceValueConverter = GetSourceValueConverter();
            objectBuilderContext = new ObjectBuilderContext(sourceValueConverter, metadataProvider);
        }

        private static SourceValueConverter GetSourceValueConverter()
        {
            var sourceValueConverter = new SourceValueConverter();
            sourceValueConverter.Add(typeof(Thickness), context => new ThicknessConverter().ConvertFrom(null, CultureInfo.CurrentCulture, context.Value));
            sourceValueConverter.Add(typeof(Brush), context => new BrushConverter().ConvertFrom(null, CultureInfo.CurrentCulture, context.Value));
            sourceValueConverter.Add(typeof(GridLength), context => new GridLengthConverter().ConvertFrom(null, CultureInfo.CurrentCulture, context.Value));
            sourceValueConverter.Add(typeof(ImageSource), context => new ImageSourceConverter().ConvertFrom(null, CultureInfo.CurrentCulture, context.Value));
            return sourceValueConverter;
        }

        public ConstructionResult Load(string xaml)
        {
            return CreateWithParams(xaml, (builder, node, context) => builder.Inflate(node, context));
        }

        public ConstructionResult Load(string xaml, object intance)
        {
            return CreateWithParams(xaml, (builder, node, context) => builder.Inflate(node, intance, context));
        }

        public ConstructionResult CreateWithParams(string xaml, Func<IObjectBuilder, ConstructionNode, BuildContext, object> createFunc)
        {
            var constructionNode = GetConstructionNode(xaml);
            var sourceValueConverter = GetSourceValueConverter();
            var instanceCreator = new InstanceCreator(sourceValueConverter, objectBuilderContext, directory);
            var objectBuilder = new ExtendedObjectBuilder(instanceCreator, objectBuilderContext, new ContextFactory(directory, objectBuilderContext));
            var namescopeAnnotator = new NamescopeAnnotator(objectBuilderContext.MetadataProvider);
            var buildContext = new BuildContext(namescopeAnnotator, new AmbientRegistrator(), new InstanceLifecycleSignaler());
            var instance = createFunc(objectBuilder, constructionNode, buildContext);
            return new ConstructionResult(instance, namescopeAnnotator);
        }

        private ConstructionNode GetConstructionNode(string xaml)
        {
            var parser = new XamlToTreeParser(new MetadataProvider(), new[] { new InlineParser(directory) }, new Resolver(directory));
            var tree = parser.Parse(xaml);
            return tree;
        }
    }
}