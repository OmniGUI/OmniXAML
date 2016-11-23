namespace OmniXaml.Tests.XmlParserTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class EventsTests : XamlToTreeParserTestsBase
    {
        [TestMethod]
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
                                Assignments = new[] {new MemberAssignment() {SourceValue = "OnClick", Member = Member.FromStandard(typeof(Button), "Click")},}
                            },
                        }
                    },
                }
            };

            Assert.AreEqual(expected, tree.Root);
        }

        [TestMethod]
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
                        SourceValue = "OnLoad",
                    },
                },
            };

            Assert.AreEqual(expected, tree.Root);
        }
    }
}