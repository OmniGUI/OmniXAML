namespace OmniXaml.Tests
{
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class ObjectBuilderTests
    {
        //[TestMethod]
        //public void ObjectAndDirectProperties()
        //{
        //    var tree = Parse("<Window Title=\"Saludos\" />");
        //}

        //private static ConstructionNode Parse(string xaml)
        //{
        //    var ass = Assembly.Load(new AssemblyName("OmniXaml.Tests"));
        //    var sut = new XamlToTreeParser(ass, new[] { "OmniXaml.Tests.Model" });
        //    var tree = sut.Parse(xaml);
        //    return tree;
        //}

        //[TestMethod]
        //public void InnerStringProperty()
        //{
        //    var tree = Parse("<Window><Window.Content>Hola</Window.Content></Window>");
        //}

        //[TestMethod]
        //public void InnerComplexProperty()
        //{
        //    var tree = Parse("<Window><Window.Content><TextBlock /></Window.Content></Window>");
        //}

        [TestMethod]
        public void ImmutableFromContent()
        {
            var node = new ConstructionNode(typeof(MyImmutable)) { InjectableArguments = new [] { "Hola"}};
            var myImmutable = new MyImmutable("Hola");
            var actual = new ObjectBuilder(new InstanceCreator(), new SourceValueConverter()).Create(node);

            Assert.AreEqual(myImmutable, actual);
        }
    }
}