namespace OmniXaml.Tests.Parsers.MarkupExtensionParserTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Builder;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.MarkupExtensions;

    [TestClass]
    public class MarkupExtensionNodeToXamlNodesConverterTests : GivenAWiringContext
    {
        private XamlNodeBuilder x;

        public MarkupExtensionNodeToXamlNodesConverterTests()
        {
            x = new XamlNodeBuilder(WiringContext.TypeContext);
        }

        [TestMethod]
        public void NameOnly()
        {
            var tree = new MarkupExtensionNode(new IdentifierNode("DummyExtension"));
            var sut = new MarkupExtensionNodeToXamlNodesConverter(WiringContext);
            var actualNodes = sut.Convert(tree).ToList();
            var expectedNodes = new List<XamlNode>
            {
                x.StartObject<DummyExtension>(),
                x.EndObject(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void NameAndAttribute()
        {
            var tree = new MarkupExtensionNode(new IdentifierNode("DummyExtension"), new OptionsCollection {new PropertyOption("Property", new StringNode("Value"))});
            var sut = new MarkupExtensionNodeToXamlNodesConverter(WiringContext);
            var actualNodes = sut.Convert(tree).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.StartObject<DummyExtension>(),
                x.StartMember<DummyExtension>(d => d.Property),
                x.Value("Value"),
                x.EndMember(),
                x.EndObject(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void NameAndTwoAttributes()
        {
            var tree = new MarkupExtensionNode(new IdentifierNode("DummyExtension"), new OptionsCollection
            {
                new PropertyOption("Property", new StringNode("Value")),
                new PropertyOption("AnotherProperty", new StringNode("AnotherValue")),
            });
            var sut = new MarkupExtensionNodeToXamlNodesConverter(WiringContext);
            var actualNodes = sut.Convert(tree).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.StartObject<DummyExtension>(),
                x.StartMember<DummyExtension>(d => d.Property),
                x.Value("Value"),
                x.EndMember(),
                x.StartMember<DummyExtension>(d => d.AnotherProperty),
                x.Value("AnotherValue"),
                x.EndMember(),
                x.EndObject(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void PositionalOption()
        {
            var tree = new MarkupExtensionNode(new IdentifierNode("DummyExtension"), new OptionsCollection
            {
               new PositionalOption("Option")
            });
            var sut = new MarkupExtensionNodeToXamlNodesConverter(WiringContext);
            var actualNodes = sut.Convert(tree).ToList();

            var expectedNodes = new Collection<XamlNode>
            {
                x.StartObject<DummyExtension>(),
                x.MarkupExtensionArguments(),
                x.Value("Option"),
                x.EndMember(),
                x.EndObject(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }
    }

   
}
