namespace OmniXaml.Tests.ObjectBuilderTests
{
    using Glass.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class Dictionaries : ObjectBuilderTestsBase
    {
        [TestMethod]
        public void Dictionary()
        {
            var node = new ConstructionNode(typeof(ResourceDictionary))
            {
                Children = new[] { new ConstructionNode(typeof(TextBlock)) { Key = "MyKey" }, }
            };

            var actual = (ResourceDictionary)Create(node).Result;

            var expected = new ResourceDictionary { {"MyKey", new TextBlock() } };
            Assert.IsTrue(expected.ContentEquals(actual));
        }     
    }
}