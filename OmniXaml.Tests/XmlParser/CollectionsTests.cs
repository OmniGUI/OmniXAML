namespace OmniXaml.Tests.XmlParser
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class CollectionsTests : XamlToTreeParserTestsBase
    {
        [TestMethod]
        public void EmptyCollection()
        {
            var tree = Parse(@"<Collection xmlns=""root""></Collection>");
            Assert.AreEqual(new ConstructionNode(typeof(Collection)), tree);
        }

        [TestMethod]
        public void Collection()
        {
            var tree = Parse(@"<Collection xmlns=""root""><TextBlock/></Collection>");
            Assert.AreEqual(
                new ConstructionNode(typeof(Collection))
                {
                    Assignments = new[] { new MemberAssignment { Children = new[] { new ConstructionNode(typeof(TextBlock)) } } }
                },
                tree);
        }
    }
}