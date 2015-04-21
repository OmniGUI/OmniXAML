namespace OmniXaml.Tests.MarkupExtensionParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Parsers.Sprache.MarkupExtension;
    using Sprache;
    using MarkupExtensionParser = Parsers.Sprache.MarkupExtension.MarkupExtensionParser;

    [TestClass]
    public class ParsingTests
    {
        [TestMethod]
        public void SimpleExtension()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{Dummy}");
            Assert.AreEqual(new MarkupExtensionNode(new IdentifierNode("Dummy")), actual);
        }

        [TestMethod]
        public void PrefixedExtension()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{x:Dummy}");
            Assert.AreEqual(new MarkupExtensionNode(new IdentifierNode("x", "Dummy")), actual);
        }

        [TestMethod]
        public void ExtensionWithTwoPositionalOptions()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{Dummy Value1,Value2}");
            var options = new OptionsCollection { new PositionalOption("Value1"), new PositionalOption("Value2") };
            Assert.AreEqual(new MarkupExtensionNode(new IdentifierNode("Dummy"), options), actual);
        }

        [TestMethod]
        public void DelimitedBy()
        {
            var identifier = from c in Parse.LetterOrDigit.Many() select new string(c.ToArray());

            var parser = from id in identifier.DelimitedBy(Parse.Char(',').Token()) select id;
            var p = parser.Parse("SomeValue   ,  AnotherValue");
            CollectionAssert.AreEqual(new[] { "SomeValue", "AnotherValue" }, p.ToList());
        }

        [TestMethod]
        public void ExtensionWithPositionalAndAssignmentOptions()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{Dummy Value,Property='Some Value'}");
            var options = new OptionsCollection { new PositionalOption("Value"), new PropertyOption("Property", new StringNode("Some Value")) };
            Assert.AreEqual(new MarkupExtensionNode(new IdentifierNode("Dummy"), options), actual);
        }

        [TestMethod]
        public void AssignmentOfDirectValue()
        {
            var actual = MarkupExtensionParser.Assignment.Parse("Property=SomeValue");
            Assert.AreEqual(new AssignmentNode("Property", new StringNode("SomeValue")), actual);
        }

        [TestMethod]
        public void AssignmentOfQuotedValue()
        {
            var actual = MarkupExtensionParser.Assignment.Parse("Property='value with spaces'");
            Assert.AreEqual(new AssignmentNode("Property", new StringNode("value with spaces")), actual);
        }

        [TestMethod]
        public void ParsePositionaAndPropertyOptions()
        {
            var actual = MarkupExtensionParser.Options.Parse("value1,Property1=Value1,Property2='Some value'");
            var expected = new OptionsCollection(new List<Option>
            {
                new PositionalOption("value1"),
                new PropertyOption("Property1", new StringNode("Value1")),
                new PropertyOption("Property2", new StringNode("Some value"))
            });

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ParsePropertyWithExtension()
        {
            var actual = MarkupExtensionParser.Assignment.Parse("Value={Dummy}");
            var markupExtensionNode = new MarkupExtensionNode(new IdentifierNode("Dummy"));
            Assert.AreEqual(new AssignmentNode("Value", markupExtensionNode), actual);
        }

        [TestMethod]
        public void ComposedExtension()
        {
            var actual =
                MarkupExtensionParser.MarkupExtension.Parse(
                    "{Binding Width, RelativeSource={RelativeSource FindAncestor, AncestorLevel=1, AncestorType={x:Type Grid}}}");

            var expected = new MarkupExtensionNode(
                new IdentifierNode("Binding"),
                new OptionsCollection(
                    new List<Option>
                    {
                        new PositionalOption("Width"),
                        new PropertyOption(
                            "RelativeSource",
                            new MarkupExtensionNode(
                                new IdentifierNode("RelativeSource"),
                                new OptionsCollection
                                {
                                    new PositionalOption("FindAncestor"),
                                    new PropertyOption("AncestorLevel", new StringNode("1")),
                                    new PropertyOption(
                                        "AncestorType",
                                        new MarkupExtensionNode(
                                            new IdentifierNode("x", "Type"),
                                            new OptionsCollection
                                            {
                                                new PositionalOption("Grid")
                                            }))

                                }))
                    }));

            Assert.AreEqual(expected, actual);
        }
    }
}
