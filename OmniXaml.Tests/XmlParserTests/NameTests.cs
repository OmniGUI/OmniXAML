namespace OmniXaml.Tests.XmlParserTests
{
    using Model;
    using Xunit;

    public class NameTests : XamlToTreeParserTestsBase
    {
        [Fact]
        public void Name()
        {
            var tree = ParseResult(@"<Window xmlns=""root"" Name=""MyWindow"" />");
            Assert.Equal(
                new ConstructionNode(typeof(Window))
                {
                    Name = "MyWindow",
                    Assignments = new[] { new MemberAssignment { Member = Member.FromStandard<Window>(window => window.Name), SourceValue = "MyWindow" } }
                },
                tree.Root);
        }

        [Fact]
        public void XName()
        {
            var tree = ParseResult(@"<Window xmlns=""root"" xmlns:x=""special"" x:Name=""MyWindow"" />");
            Assert.Equal(
                new ConstructionNode(typeof(Window))
                {
                    Name = "MyWindow",
                    Assignments = new[] { new MemberAssignment { Member = Member.FromStandard<Window>(window => window.Name), SourceValue = "MyWindow" } }
                },
                tree.Root);
        }
    }
}