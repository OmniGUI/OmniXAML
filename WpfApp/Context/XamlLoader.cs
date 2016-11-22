namespace WpfApp.Context
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using OmniXaml;
    using OmniXaml.Ambient;
    using OmniXaml.TypeLocation;

    public class XamlLoader : IXamlLoader
    {
        private readonly ITypeDirectory typeDirectory;
        private readonly ObjectBuilderContext objectBuilderContext;

        public XamlLoader()
        {
            typeDirectory = GetTypeDirectory();
            var metadataProvider = new MetadataProvider();
            var sourceValueConverter = GetSourceValueConverter();
            objectBuilderContext = new ObjectBuilderContext(sourceValueConverter, metadataProvider);
        }

        public ConstructionResult Load(string xaml)
        {
            return CreateWithParams(xaml, (builder, node, context) => builder.Inflate(node, context));
        }

        public ConstructionResult Load(string xaml, object intance)
        {
            return CreateWithParams(xaml, (builder, node, context) => builder.Inflate(node, context, intance));
        }

        private ITypeDirectory GetTypeDirectory()
        {
            var configuredAssemblyWithNamespaces = Route
                .Assembly(typeof(Window).Assembly)
                .WithNamespaces("System.Windows", "System.Windows.Controls");
            var xamlNamespace = XamlNamespace
                .Map("root")
                .With(configuredAssemblyWithNamespaces);

            return new TypeDirectory(new[] {xamlNamespace});
        }

        private static SourceValueConverter GetSourceValueConverter()
        {
            var sourceValueConverter = new SourceValueConverter();
            sourceValueConverter.Add(typeof(Thickness), context => new ThicknessConverter().ConvertFrom(null, CultureInfo.CurrentCulture, context.Value));
            sourceValueConverter.Add(typeof(Brush), context => new BrushConverter().ConvertFrom(null, CultureInfo.CurrentCulture, context.Value));
            sourceValueConverter.Add(typeof(GridLength), context => new GridLengthConverter().ConvertFrom(null, CultureInfo.CurrentCulture, context.Value));
            sourceValueConverter.Add(typeof(ImageSource), context => new BitmapImage(new Uri((string) context.Value, UriKind.Relative)));
            return sourceValueConverter;
        }

        private ConstructionResult CreateWithParams(string xaml, Func<IObjectBuilder, ConstructionNode, BuildContext, object> createFunc)
        {
            var parseResult = GetConstructionNode(xaml);
            var sourceValueConverter = GetSourceValueConverter();
            var instanceCreator = new InstanceCreator(sourceValueConverter, objectBuilderContext, typeDirectory);
            var objectBuilder = new ExtendedObjectBuilder(instanceCreator, objectBuilderContext, new ContextFactory(typeDirectory, objectBuilderContext));
            var namescopeAnnotator = new NamescopeAnnotator(objectBuilderContext.MetadataProvider);
            var buildContext = new BuildContext(namescopeAnnotator, new AmbientRegistrator(), new InstanceLifecycleSignaler())
            {
                PrefixAnnotator = parseResult.PrefixAnnotator,
                PrefixedTypeResolver = new PrefixedTypeResolver(parseResult.PrefixAnnotator, typeDirectory)
            };

            var instance = createFunc(objectBuilder, parseResult.Root, buildContext);
            return new ConstructionResult(instance, namescopeAnnotator);
        }

        private ParseResult GetConstructionNode(string xaml)
        {
            var resolver = new Resolver(typeDirectory);
            var parser = new XamlToTreeParser(new MetadataProvider(), new[] {new InlineParser(resolver) }, resolver);
            var tree = parser.Parse(xaml, new PrefixAnnotator());
            return tree;
        }
    }
}