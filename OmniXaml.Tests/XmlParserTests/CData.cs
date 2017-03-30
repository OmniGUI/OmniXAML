namespace OmniXaml.Tests.XmlParserTests
{
    using Model;
    using Xunit;

    public class CData : XamlToTreeParserTestsBase
    {
        [Fact]
        public void CDataInsidePropertyElement()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><Window.Content><![CDATA[Hello]]></Window.Content></Window>");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment()
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Children = ConstructionNode.ForString("Hello"),
                    },
                }
            };

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void CDataAsContentProperty()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><![CDATA[Hello]]></Window>");

            var expected = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment()
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Children = ConstructionNode.ForString("Hello"),
                    },
                }
            };

            Assert.Equal(expected, parseResult.Root);
        }
    }
}