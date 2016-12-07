namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System.Collections.Generic;
    using Model;
    using Xunit;

    public class Dictionaries : ObjectBuilderTestsBase
    {
        [Fact]
        public void ChildrenToDictionary()
        {
            var node = new ConstructionNode(typeof(ResourceDictionary))
            {
                Children = new[] { new ConstructionNode(typeof(TextBlock)) { Key = "MyKey" }, }
            };

            var actual = Create(node).Result;

            var expected = new ResourceDictionary { {"MyKey", new TextBlock() } };
            Assert.Equal(expected, actual);
        }

        [Fact]
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
            Assert.Equal(expected, actual);
        }
    }
}