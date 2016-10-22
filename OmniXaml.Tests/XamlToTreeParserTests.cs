using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OmniXaml.Tests
{
    using System.IO;
    using System.Reflection;
    using Metadata;
    using Model;
    using TypeLocation;

    [TestClass]
    public class XamlToTreeParserTests
    {
        [TestMethod]
        public void ObjectAndDirectProperties()
        {
            var tree = Parse("<Window Title=\"Saludos\" />");
        }

        private static ConstructionNode Parse(string xaml)
        {
            var ass = Assembly.Load(new AssemblyName("OmniXaml.Tests"));
            var directory = new TypeDirectory();
            directory.RegisterPrefix(new PrefixRegistration(string.Empty, "root"));
            directory.AddNamespace(XamlNamespace.Map("root").With(Route.Assembly(ass).WithNamespaces("OmniXaml.Tests.Model")));           

            var sut = new XamlToTreeParser(directory, Context.GetMetadataProvider(), new[] { new InlineParser(directory), });

            var tree = sut.Parse(xaml);
            return tree;
        }

        [TestMethod]
        public void InnerStringProperty()
        {
            var tree = Parse("<Window><Window.Content>Hola</Window.Content></Window>");
        }

        [TestMethod]
        public void InnerComplexProperty()
        {
            var tree = Parse("<Window><Window.Content><TextBlock /></Window.Content></Window>");
        }

        [TestMethod]
        public void ImmutableFromContent()
        {
            var tree = Parse("<MyImmutable>hola</MyImmutable>");
        }

        [TestMethod]
        public void ContentPropertyDirectContent()
        {
            var tree = Parse("<Window><TextBlock /></Window>");
        }

        [TestMethod]
        public void ContentPropertyDirectContentText()
        {
            var tree = Parse("<TextBlock>Hello</TextBlock>");
        }

        [TestMethod]
        public void ContentPropertyDirectContentTextInsideChild()
        {
            var tree = Parse("<Window><TextBlock>Saludos cordiales</TextBlock></Window>");
        }

        [TestMethod]
        public void MarkupExtension()
        {
            var tree = Parse(@"<Window Content=""{Simple}"" />");
        }
    }
}
