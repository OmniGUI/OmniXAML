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

            var expected = new ConstructionNode<Window>().WithAssignment(w => w.Content, "Hello");

            Assert.Equal(expected, parseResult.Root);
        }

        [Fact]
        public void CDataAsContentProperty()
        {
            var parseResult = ParseResult(@"<Window xmlns=""root""><![CDATA[Hello]]></Window>");

            var expected = new ConstructionNode<Window>().WithAssignment(w => w.Content, "Hello");
            
            Assert.Equal(expected, parseResult.Root);
        }
    }
}