namespace OmniXaml.Tests.XmlParserTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class CData : XamlToTreeParserTestsBase
    {
        [TestMethod]
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
                        SourceValue = "Hello"
                    },
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }

        [TestMethod]
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
                        SourceValue = "Hello"
                    },
                }
            };

            Assert.AreEqual(expected, parseResult.Root);
        }
    }
}