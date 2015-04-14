namespace OmniXaml.Tests.MarkupExtensionParser
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sprache;
    using MarkupExtensionParser = OmniXaml.MarkupExtensionParser;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void SimpleExtension()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{Dummy}");
            Assert.AreEqual(new MarkupExtensionNode("Dummy"), actual);
        }

        [TestMethod]
        public void ExtensionWithTwoPositionalOptions()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{Dummy Value1,Value2}");
            var options = new List<Option> {new PositionalOption("Value1"), new PositionalOption("Value2")};
            Assert.AreEqual(new MarkupExtensionNode("Dummy", options), actual);
        }

        [TestMethod]
        public void ExtensionWithPositionalAndAssignmentOptions()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{Dummy Value,Property='Some Value'}");
            var options = new List<Option> { new PositionalOption("Value"), new PropertyOption("Property", "Some Value") };
            Assert.AreEqual(new MarkupExtensionNode("Dummy", options), actual);
        }

        [TestMethod]
        public void AssignmentOfDirectValue()
        {
            var actual = MarkupExtensionParser.Assignment.Parse("Property=SomeValue");
            Assert.AreEqual(new AssignmentNode("Property", "SomeValue"), actual);
        }    

        [TestMethod]
        public void AssignmentOfQuotedValue()
        {
            var actual = MarkupExtensionParser.Assignment.Parse("Property='value with spaces'");
            Assert.AreEqual(new AssignmentNode("Property", "value with spaces"), actual);
        }

        [TestMethod]
        public void ParsePositionaAndPropertyOptions()
        {
            var actual = MarkupExtensionParser.Options.Parse("value1,Property1=Value1,Property2='Some value'");
            var expected = new OptionsCollection(new List<Option>
            {
                new PositionalOption("value1"),
                new PropertyOption("Property1", "Value1"),
                new PropertyOption("Property2", "Some value")
            });

            Assert.AreEqual(actual, expected);
        }
    }
}
