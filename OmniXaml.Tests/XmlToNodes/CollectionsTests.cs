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
            Assert.Equal(new ConstructionNode<Collection>(), tree.Root);
        }

        [Fact]
        public void Collection()
        {
            var tree = ParseResult(@"<Collection Title=""My title"" xmlns=""root""><TextBlock/></Collection>");
            var memberAssignments = new[]
            {
                new MemberAssignment
                {
                    Member = Member.FromStandard<Collection>(collection => collection.Title),
                    SourceValue = "My title"
                }
            };
            var children = new[]
            {
                new ConstructionNode<TextBlock>()
            };
            var expected = new ConstructionNode<Collection>()
                .WithAssignments(memberAssignments)
                .WithChildren(children);

            Assert.Equal(expected, tree.Root);
        }

        [Fact]
        public void PropertyThatIsCollection()
        {
            var tree = ParseResult(@"<ItemsControl xmlns=""root""><TextBlock/><TextBlock/><TextBlock/></ItemsControl>");
            var memberAssignments = new[]
            {
                new MemberAssignment
                {
                    Member = Member.FromStandard<ItemsControl>(collection => collection.Items),
                    Values = new[]
                    {
                        new ConstructionNode<TextBlock>(),
                        new ConstructionNode<TextBlock>(),
                        new ConstructionNode<TextBlock>(),
                    }
                }
            };
            var expected = new ConstructionNode<ItemsControl>().WithAssignments(memberAssignments);

            Assert.Equal(expected, tree.Root);
        }
    }
}