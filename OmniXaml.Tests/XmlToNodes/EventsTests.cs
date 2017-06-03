using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests.XmlToNodes
{
    public class EventsTests : XamlToTreeParserTestsBase
    {
        [Fact]
        public void BasicEvent()
        {
            var tree = ParseResult(@"<Window xmlns=""root"">
                                <Button Click=""OnClick"" />
                               </Window>");

            var expected = new ConstructionNode<Window>()
                .WithAssignment(window => window.Content, new ConstructionNode<Button>()
                    .WithAssignment("Click", "OnClick"));


            Assert.Equal(expected, tree.Root);
        }

        [Fact]
        public void AttachedEvent()
        {
            var tree = ParseResult(@"<Window xmlns=""root"" Window.Loaded=""OnLoad"" />");

            var expected = new ConstructionNode<Window>
            {
            }.WithAssignments(new[]
            {
                new MemberAssignment()
                {
                    Member = Member.FromAttached(typeof(Window), "Loaded"),
                    SourceValue = "OnLoad"
                },
            });

            Assert.Equal(expected, tree.Root);
        }
    }
}