namespace OmniXaml.Tests.XmlParserTests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;
    using Model.Custom;

    [TestClass]
    public class ClassDirective : XamlToTreeParserTestsBase
    {
        [TestMethod]
        public void Class()
        {
            var xaml = @"<Window xmlns=""root"" xmlns:x=""special"" x:Class=""OmniXaml.Tests.Model.Custom.CustomWindow"" />";
            var p = ParseResult(xaml);
            ConstructionNode expected = new ConstructionNode(typeof(Window)) { InstantiateAs = typeof(CustomWindow) };

            Assert.AreEqual(expected, p.Root);
        }

        [TestMethod]
        public void EventAttachedToCorrectInstance()
        {
            var xaml = @"<Window xmlns=""root"" xmlns:x=""special"" x:Class=""OmniXaml.Tests.Model.TestWindow"" Clicked=""OnClick"" />";
            var p = ParseResult(xaml);
            var expected = new ConstructionNode(typeof(Window))
            {
                InstantiateAs = typeof(TestWindow),
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard(typeof(TestWindow), "Clicked"),
                        SourceValue = "OnClick",
                    }
                }
            };

            Assert.AreEqual(expected, p.Root);
        }
    }
}