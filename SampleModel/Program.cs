namespace SampleModel
{
    using System.IO;
    using System.Reflection;
    using Model;
    using OmniXaml;
    using OmniXaml.Metadata;
    using OmniXaml.TypeLocation;

    public class Program
    {
        public static void Main(string[] args)
        {
            var xaml = File.ReadAllText("Model.xml");
            var ctNode = Parse(xaml);
            var result= Construct(ctNode);
            var rocky = result.NamescopeAnnotator.Find("Rocky", result.Instance);
        }

        private static ConstructionResult Construct(ConstructionNode ctNode)
        {
            var objectConstructor = new ObjectBuilder(new ConstructionContext(new InstanceCreator(), new SourceValueConverter(), new MetadataProvider(), new InstanceLifecycleSignaler()));
            var namescopeAnnotator = new NamescopeAnnotator();
            var construct = objectConstructor.Create(ctNode, new CreationContext(namescopeAnnotator, null));
            return new ConstructionResult(construct, namescopeAnnotator);
        }

        private static ConstructionNode Parse(string xaml)
        {
            var assembly = Assembly.Load(new AssemblyName("SampleModel"));
            var directory = new TypeDirectory();

            directory.AddNamespace(XamlNamespace.Map("root").With(Route.Assembly(assembly).WithNamespaces("SampleModel.Model")));

            var sut = new XamlToTreeParser(directory, Context.GetMetadataProvider(), new[] { new InlineParser(directory) });

            var tree = sut.Parse(xaml);
            return tree;
        }
    }
}
