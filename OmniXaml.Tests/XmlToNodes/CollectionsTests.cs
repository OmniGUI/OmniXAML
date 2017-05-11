using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests.XmlToNodes
{
    public class CollectionsTests : XamlToTreeParserTestsBase
    {
        [Fact]
        public void EmptyCollection()
        {
            var tree = ParseResult(@"<Collection xmlns=""root""></Collection>");
            Assert.Equal(new ConstructionNode(typeof(Collection)), tree.Root);
        }

        [Fact]
        public void Collection()
        {
            var tree = ParseResult(@"<Collection Title=""My title"" xmlns=""root""><TextBlock/></Collection>");
            var expected = new ConstructionNode(typeof(Collection))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Collection>(collection => collection.Title),
                        SourceValue = "My title"                        
                    }
                },
                Children = new[]
                {
                    new ConstructionNode(typeof(TextBlock))
                }
            };

            Assert.Equal(expected, tree.Root);
        }

        [Fact]
        public void PropertyThatIsCollection()
        {
            var tree = ParseResult(@"<ItemsControl xmlns=""root""><TextBlock/><TextBlock/><TextBlock/></ItemsControl>");
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

            Assert.Equal(expected, tree.Root);
        }
    }
}