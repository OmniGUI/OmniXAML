namespace OmniXaml.Tests.XmlParserTests
{
    using System.Reflection;
    using DefaultLoader;
    using TypeLocation;

    public class XamlToTreeParserTestsBase
    {
        protected static ConstructionNode Parse(string xaml)
        {
            var ass = Assembly.Load(new AssemblyName("OmniXaml.Tests"));
            var directory = new TypeDirectory();
            directory.RegisterPrefix(new PrefixRegistration(string.Empty, "root"));
            directory.AddNamespace(XamlNamespace.Map("root").With(Route.Assembly(ass).WithNamespaces("OmniXaml.Tests.Model")));
            directory.AddNamespace(XamlNamespace.Map("custom").With(Route.Assembly(ass).WithNamespaces("OmniXaml.Tests.Model.Custom")));

            var sut = new XamlToTreeParser(directory, new AttributeBasedMetadataProvider(), new[] {new InlineParser(directory)});

            var tree = sut.Parse(xaml);
            return tree;
        }
    }
}