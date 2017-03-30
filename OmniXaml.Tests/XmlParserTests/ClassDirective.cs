namespace OmniXaml.Tests.XmlParserTests
{
    using System.Collections.Generic;
    using Model;
    using Model.Custom;
    using Xunit;

    public class ClassDirective : XamlToTreeParserTestsBase
    {
        [Fact]
        public void Class()
        {
            var xaml = @"<Window xmlns=""root"" xmlns:x=""special"" x:Class=""OmniXaml.Tests.Model.Custom.CustomWindow;assembly=OmniXaml.Tests"" />";
            var p = ParseResult(xaml);
            ConstructionNode expected = new ConstructionNode(typeof(Window)) { InstantiateAs = typeof(CustomWindow) };

            Assert.Equal(expected, p.Root);
        }

        [Fact]
        public void EventAttachedToCorrectInstance()
        {
            var xaml = @"<Window xmlns=""root"" xmlns:x=""special"" x:Class=""OmniXaml.Tests.Model.TestWindow;assembly=OmniXaml.Tests"" Clicked=""OnClick"" />";
            var p = ParseResult(xaml);
            var expected = new ConstructionNode(typeof(Window))
            {
                InstantiateAs = typeof(TestWindow),
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard(typeof(TestWindow), "Clicked"),
                        Children = ConstructionNode.ForString("OnClick"),
                    }
                }
            };

            Assert.Equal(expected, p.Root);
        }
    }
}