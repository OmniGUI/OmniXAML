namespace OmniXaml.Tests.Parsers.MarkupExtensionParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.MarkupExtensions;
    using Resources;
    using Sprache;

    [TestClass]
    public class ParsingTests
    {
        [TestMethod]
        public void SimpleExtension()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{Dummy}");
            Assert.AreEqual(new MarkupExtensionNode(new IdentifierNode("DummyExtension")), actual);
        }

        [TestMethod]
        public void PrefixedExtension()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{x:Dummy}");
            Assert.AreEqual(new MarkupExtensionNode(new IdentifierNode("x", "DummyExtension")), actual);
        }

        [TestMethod]
        public void ExtensionWithTwoPositionalOptions()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{Dummy Value1,Value2}");
            var options = new OptionsCollection { new PositionalOption("Value1"), new PositionalOption("Value2") };
            Assert.AreEqual(new MarkupExtensionNode(new IdentifierNode("DummyExtension"), options), actual);
        }

        [TestMethod]
        public void ExtensionWithDottedPositionalOption()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{Dummy Direct.Value}");
            var options = new OptionsCollection { new PositionalOption("Direct.Value"), };
            Assert.AreEqual(new MarkupExtensionNode(new IdentifierNode("DummyExtension"), options), actual);
        }

        [TestMethod]
        public void DelimitedBy()
        {
            var identifier = from c in Parse.LetterOrDigit.Many() select new string(c.ToArray());

            var parser = from id in identifier.DelimitedBy(Parse.Char(',').Token()) select id;
            var parsed = parser.Parse("SomeValue   ,  AnotherValue");
            CollectionAssert.AreEqual(new[] { "SomeValue", "AnotherValue" }, parsed.ToList());
        }

        [TestMethod]
        public void ExtensionWithPositionalAndAssignmentOptions()
        {
            var actual = MarkupExtensionParser.MarkupExtension.Parse("{Dummy Value,Property='Some Value'}");
            var options = new OptionsCollection { new PositionalOption("Value"), new PropertyOption("Property", new StringNode("Some Value")) };
            Assert.AreEqual(new MarkupExtensionNode(new IdentifierNode("DummyExtension"), options), actual);
        }

        [TestMethod]
        public void AssignmentOfDirectValue()
        {
            var actual = MarkupExtensionParser.Assignment.Parse("Property=SomeValue");
            Assert.AreEqual(new AssignmentNode("Property", new StringNode("SomeValue")), actual);
        }

        [TestMethod]
        public void AssignmentOfDottedDirectValue()
        {
            var actual = MarkupExtensionParser.Assignment.Parse("Property=Some.Value");
            Assert.AreEqual(new AssignmentNode("Property", new StringNode("Some.Value")), actual);
        }

        [TestMethod]
        public void AssignmentOfDirectValueWithColon()
        {
            var actual = MarkupExtensionParser.Assignment.Parse("Property=x:SomeValue");
            Assert.AreEqual(new AssignmentNode("Property", new StringNode("x:SomeValue")), actual);
        }

        [TestMethod]
        public void AssignmentOfQuotedValue()
        {
            var actual = MarkupExtensionParser.Assignment.Parse("Property='value with spaces'");
            Assert.AreEqual(new AssignmentNode("Property", new StringNode("value with spaces")), actual);
        }

        [TestMethod]
        public void ParsePositionalAndPropertyOptions()
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
            var markupExtensionNode = new MarkupExtensionNode(new IdentifierNode("DummyExtension"));
            Assert.AreEqual(new AssignmentNode("Value", markupExtensionNode), actual);
        }

        [TestMethod]
        public void ComposedExtensionTemplateBindingWithConverter()
        {
            var actual =
               MarkupExtensionParser.MarkupExtension.Parse("{TemplateBinding Path=IsFloatingWatermarkVisible, Converter={Type FooBar}}");

            var expected = MarkupExtensionNodeResources.ComposedExtensionTemplateBindingWithConverter();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ComposedExtension()
        {
            var actual =
                MarkupExtensionParser.MarkupExtension.Parse(
                    "{Binding Width, RelativeSource={RelativeSource FindAncestor, AncestorLevel=1, AncestorType={x:Type Grid}}}");

            var expected = MarkupExtensionNodeResources.ComposedExtension();

            Assert.AreEqual(expected, actual);
        }
    }
}
