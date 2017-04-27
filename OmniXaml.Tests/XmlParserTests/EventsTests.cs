namespace OmniXaml.Tests.XmlParserTests
{
    using Model;
    using Xunit;

    public class EventsTests : XamlToTreeParserTestsBase
    {
        [Fact]
        public void BasicEvent()
        {
            var tree = ParseResult(@"<Window xmlns=""root"">
                                <Button Click=""OnClick"" />
                               </Window>");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment()
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Children = new[]
                        {
                            new ConstructionNode(typeof(Button))
                            {
                                Assignments = new[] {new MemberAssignment()
                                {
                                    Member = Member.FromStandard(typeof(Button), "Click"),
                                    SourceValue = "OnClick"
                                },}
                            },
                        }
                    },
                }
            };

            Assert.Equal(expected, tree.Root);
        }

        [Fact]
        public void AttachedEvent()
        {
            var tree = ParseResult(@"<Window xmlns=""root"" Window.Loaded=""OnLoad"" />");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment()
                    {
                        Member = Member.FromAttached(typeof(Window), "Loaded"),
                        SourceValue = "OnLoad"
                    },
                },
            };

            Assert.Equal(expected, tree.Root);
        }
    }
}