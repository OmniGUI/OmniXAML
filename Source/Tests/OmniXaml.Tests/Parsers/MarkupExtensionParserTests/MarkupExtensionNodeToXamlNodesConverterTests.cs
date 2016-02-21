namespace OmniXaml.Tests.Parsers.MarkupExtensionParserTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Common.DotNetFx;
    using Xunit;
    using OmniXaml.Parsers.MarkupExtensions;
    using Resources;

    public class MarkupExtensionNodeToXamlNodesConverterTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {     
        [Fact]
        public void NameOnly()
        {
            var tree = new MarkupExtensionNode(new IdentifierNode("DummyExtension"));
            var sut = new MarkupExtensionNodeToXamlNodesConverter(RuntimeTypeSource);
            var actualNodes = sut.ParseMarkupExtensionNode(tree).ToList();
            var expectedInstructions = new List<Instruction>
            {
                X.StartObject<DummyExtension>(),
                X.EndObject(),
            };

            Assert.Equal(expectedInstructions, actualNodes);
        }

        [Fact]
        public void NameAndAttribute()
        {
            var tree = new MarkupExtensionNode(new IdentifierNode("DummyExtension"), new OptionsCollection {new PropertyOption("Property", new StringNode("Value"))});
            var sut = new MarkupExtensionNodeToXamlNodesConverter(RuntimeTypeSource);
            var actualNodes = sut.ParseMarkupExtensionNode(tree).ToList();

            var expectedInstructions = new List<Instruction>
            {
                X.StartObject<DummyExtension>(),
                X.StartMember<DummyExtension>(d => d.Property),
                X.Value("Value"),
                X.EndMember(),
                X.EndObject(),
            };

            Assert.Equal(expectedInstructions, actualNodes);
        }

        [Fact]
        public void NameAndTwoAttributes()
        {
            var tree = new MarkupExtensionNode(new IdentifierNode("DummyExtension"), new OptionsCollection
            {
                new PropertyOption("Property", new StringNode("Value")),
                new PropertyOption("AnotherProperty", new StringNode("AnotherValue")),
            });
            var sut = new MarkupExtensionNodeToXamlNodesConverter(RuntimeTypeSource);
            var actualNodes = sut.ParseMarkupExtensionNode(tree).ToList();

            var expectedInstructions = new List<Instruction>
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

            Assert.Equal(expectedInstructions, actualNodes);
        }

        [Fact]
        public void PositionalOption()
        {
            var tree = new MarkupExtensionNode(new IdentifierNode("DummyExtension"), new OptionsCollection
            {
               new PositionalOption("Option")
            });
            var sut = new MarkupExtensionNodeToXamlNodesConverter(RuntimeTypeSource);
            var actualNodes = sut.ParseMarkupExtensionNode(tree).ToList();

            var expectedInstructions = new Collection<Instruction>
            {
                X.StartObject<DummyExtension>(),
                X.MarkupExtensionArguments(),
                X.Value("Option"),
                X.EndMember(),
                X.EndObject(),
            };

            Assert.Equal(expectedInstructions, actualNodes);
        }

        [Fact]
        public void ComposedExtensionTemplateBindingWithConverter()
        {
            var tree = MarkupExtensionNodeResources.ComposedExtensionTemplateBindingWithConverter();
            var sut = new MarkupExtensionNodeToXamlNodesConverter(RuntimeTypeSource);
            var actualNodes = sut.ParseMarkupExtensionNode(tree).ToList();

            var expectedInstructions = new Collection<Instruction>
            {
                X.StartObject<TemplateBindingExtension>(),
                    X.StartMember<TemplateBindingExtension>(d => d.Path),
                        X.Value("IsFloatingWatermarkVisible"),
                    X.EndMember(),
                    X.StartMember<TemplateBindingExtension>(d => d.Converter),
                        X.StartObject<TypeExtension>(),
                            X.MarkupExtensionArguments(),
                                X.Value("FooBar"),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),                    
                X.EndObject(),
            };

            Assert.Equal(expectedInstructions, actualNodes);
        }
    }    
}
