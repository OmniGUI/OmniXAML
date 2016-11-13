namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System.Collections.Generic;
    using Glass.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class Dictionaries : ObjectBuilderTestsBase
    {
        [TestMethod]
        public void ChildrenToDictionary()
        {
            var node = new ConstructionNode(typeof(ResourceDictionary))
            {
                Children = new[] { new ConstructionNode(typeof(TextBlock)) { Key = "MyKey" }, }
            };

            var actual = Create(node).Result;

            var expected = new ResourceDictionary { {"MyKey", new TextBlock() } };
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PropertyToDictionary()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<MemberAssignment>()
                {
                    new MemberAssignment()
                    {
                        Member = Member.FromStandard<Window>(window => window.Resources),
                        Children = new[] { new ConstructionNode(typeof(TextBlock)) { Key = "MyKey" }, },
                    }
                }
            };

            var actual = Create(node).Result;

            var expected = new Window {Resources = new ResourceDictionary { { "MyKey", new TextBlock() } }};
            Assert.AreEqual(expected, actual);
        }
    }
}