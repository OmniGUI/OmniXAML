namespace OmniXaml.Tests
{
    using System.Collections.Generic;
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
        public void GivenSimpleExtensionThatProvidesAString_TheStringIsProvided()
        {
            var constructionNode = new ConstructionNode(typeof(SimpleExtension))
            {
                Assignments =
                    new List<PropertyAssignment>
                    {
                        new PropertyAssignment
                        {
                            Property = Property.RegularProperty<SimpleExtension>(extension => extension.Property),
                            SourceValue = "MyText"
                        }
                    }
            };

            var node = new ConstructionNode(typeof(TextBlock)) { Assignments = new []{new PropertyAssignment()
            {
                Property = Property.RegularProperty<TextBlock>(tb => tb.Text),
                Children = new []{ constructionNode}
            }, }};

            var b = Create(node);

            Assert.AreEqual(new TextBlock() { Text = " MyText"}, b);
        }

        [TestMethod]
        public void ImmutableFromContent()
        {
            var node = new ConstructionNode(typeof(MyImmutable)) { InjectableArguments = new [] { "Hola"}};
            var myImmutable = new MyImmutable("Hola");
            var actual = Create(node);

            Assert.AreEqual(myImmutable, actual);
        }

        private static object Create(ConstructionNode node)
        {
            return new ObjectBuilder(new InstanceCreator(), new SourceValueConverter()).Create(node);
        }
    }
}