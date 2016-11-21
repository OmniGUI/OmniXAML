namespace OmniXaml.Tests.XmlParserTests
{
    using System.Reflection;
    using DefaultLoader;
    using Namespaces;
    using TypeLocation;

    public class XamlToTreeParserTestsBase
    {
        protected static ParseResult Parse(string xaml)
        {
            var ass = Assembly.Load(new AssemblyName("OmniXaml.Tests"));
           
            var namespaces = new[]
            {
                XamlNamespace.Map("root").With(Route.Assembly(ass).WithNamespaces("OmniXaml.Tests.Model")),
                XamlNamespace.Map("custom").With(Route.Assembly(ass).WithNamespaces("OmniXaml.Tests.Model.Custom")),
            };

            var directory = new TypeDirectory(namespaces);
            
            var sut = new XamlToTreeParser(new AttributeBasedMetadataProvider(), new[] {new InlineParser(directory)}, new Resolver(directory));

            var prefixAnnotator = new PrefixAnnotator();
            var tree = sut.Parse(xaml, prefixAnnotator );
            return tree;
        }
    }
}