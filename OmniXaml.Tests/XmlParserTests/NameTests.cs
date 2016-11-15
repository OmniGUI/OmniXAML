namespace OmniXaml.Tests.XmlParserTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class NameTests : XamlToTreeParserTestsBase
    {
        [TestMethod]
        public void Name()
        {
            var tree = Parse(@"<Window xmlns=""root"" Name=""MyWindow"" />");
            Assert.AreEqual(
                new ConstructionNode(typeof(Window))
                {
                    Name = "MyWindow",
                    Assignments = new[] { new MemberAssignment { Member = Member.FromStandard<Window>(window => window.Name), SourceValue = "MyWindow" } }
                },
                tree);
        }

        [TestMethod]
        public void XName()
        {
            var tree = Parse(@"<Window xmlns=""root"" xmlns:x=""special"" x:Name=""MyWindow"" />");
            Assert.AreEqual(
                new ConstructionNode(typeof(Window))
                {
                    Name = "MyWindow",
                    Assignments = new[] { new MemberAssignment { Member = Member.FromStandard<Window>(window => window.Name), SourceValue = "MyWindow" } }
                },
                tree);
        }
    }
}