using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests.XmlToNodes
{
    public class CData : XamlToTreeParserTestsBase
    {
        [Fact]
        public void CDataInsidePropertyElement()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><Window.Content><![CDATA[Hello]]></Window.Content></Window>");

            var memberAssignments = new[]
            {
                new MemberAssignment()
                {
                    Member = Member.FromStandard<Window>(window => window.Content),
                    SourceValue = "Hello"
                },
            };
            var expected = new ConstructionNode(typeof(Window))
            {
            }.WithAssignments(memberAssignments);

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void CDataAsContentProperty()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><![CDATA[Hello]]></Window>");

            var memberAssignments = new[]
            {
                new MemberAssignment()
                {
                    Member = Member.FromStandard<Window>(window => window.Content),
                    SourceValue = "Hello"
                },
            };
            var expected = new ConstructionNode(typeof(Window))
            {
            }.WithAssignments(memberAssignments);

            Assert.Equal(expected, parseResult.Root);
        }
    }
}