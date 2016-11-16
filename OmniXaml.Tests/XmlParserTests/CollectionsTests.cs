namespace OmniXaml.Tests.XmlParserTests
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
            var tree = Parse(@"<Collection Title=""My title"" xmlns=""root""><TextBlock/></Collection>");
            var expected = new ConstructionNode(typeof(Collection))
            {
                Assignments = new[]
                {
                    new MemberAssignment {SourceValue = "My title", Member = Member.FromStandard<Collection>(collection => collection.Title)}
                },
                Children = new[]
                {
                    new ConstructionNode(typeof(TextBlock))
                }
            };

            Assert.AreEqual(expected, tree);
        }

        [TestMethod]
        public void PropertyThatIsCollection()
        {
            var tree = Parse(@"<ItemsControl xmlns=""root""><TextBlock/><TextBlock/><TextBlock/></ItemsControl>");
            var expected = new ConstructionNode(typeof(ItemsControl))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<ItemsControl>(collection => collection.Items),
                        Children = new[]
                        {
                            new ConstructionNode(typeof(TextBlock)),
                            new ConstructionNode(typeof(TextBlock)),
                            new ConstructionNode(typeof(TextBlock)),
                        }
                    }
                },
            };

            Assert.AreEqual(expected, tree);
        }
    }
}