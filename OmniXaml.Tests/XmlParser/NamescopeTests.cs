namespace OmniXaml.Tests.XmlParser
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class NamescopeTests : XamlToTreeParserTestsBase
    {
        [TestMethod]
        public void Namescope()
        {
            var actualNode = Parse(@"<Window xmlns:x=""special"" xmlns=""root"" ><TextBlock x:Name=""One"" /></Window>");
            var expectedNode = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(w => w.Content),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode(typeof(TextBlock))
                            {
                                Name = "One",
                                Assignments = new[] {new MemberAssignment {Member = Member.FromStandard<TextBlock>(block => block.Name), SourceValue = "One"}}
                            }
                        }
                    }
                }
            };

            Assert.AreEqual(expectedNode, actualNode);
        }
    }
}