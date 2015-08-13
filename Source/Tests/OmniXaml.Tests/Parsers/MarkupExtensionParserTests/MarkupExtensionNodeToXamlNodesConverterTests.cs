namespace OmniXaml.Tests.Parsers.MarkupExtensionParserTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Common;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.MarkupExtensions;

    [TestClass]
    public class MarkupExtensionNodeToXamlNodesConverterTests : GivenAWiringContextWithNodeBuildersNetCore
    {     
        [TestMethod]
        public void NameOnly()
        {
            var tree = new MarkupExtensionNode(new IdentifierNode("DummyExtension"));
            var sut = new MarkupExtensionNodeToXamlNodesConverter(WiringContext);
            var actualNodes = sut.Convert(tree).ToList();
            var expectedInstructions = new List<XamlInstruction>
            {
                X.StartObject<DummyExtension>(),
                X.EndObject(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void NameAndAttribute()
        {
            var tree = new MarkupExtensionNode(new IdentifierNode("DummyExtension"), new OptionsCollection {new PropertyOption("Property", new StringNode("Value"))});
            var sut = new MarkupExtensionNodeToXamlNodesConverter(WiringContext);
            var actualNodes = sut.Convert(tree).ToList();

            var expectedInstructions = new List<XamlInstruction>
            {
                X.StartObject<DummyExtension>(),
                X.StartMember<DummyExtension>(d => d.Property),
                X.Value("Value"),
                X.EndMember(),
                X.EndObject(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
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

            var expectedInstructions = new List<XamlInstruction>
            {
                X.StartObject<DummyExtension>(),
                X.StartMember<DummyExtension>(d => d.Property),
                X.Value("Value"),
                X.EndMember(),
                X.StartMember<DummyExtension>(d => d.AnotherProperty),
                X.Value("AnotherValue"),
                X.EndMember(),
                X.EndObject(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
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

            var expectedInstructions = new Collection<XamlInstruction>
            {
                X.StartObject<DummyExtension>(),
                X.MarkupExtensionArguments(),
                X.Value("Option"),
                X.EndMember(),
                X.EndObject(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }
    }

   
}
