namespace OmniXaml.Tests.Parsers.InstructionParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Common.DotNetFx;
    using Xunit;
    using OmniXaml.Parsers.Parser;

    public class MarkupExtensionsParsingFromProtoToXaml : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {
        private readonly IInstructionParser sut;
        
        public MarkupExtensionsParsingFromProtoToXaml()
        {            
            sut = new InstructionParser(RuntimeTypeSource);
        }

        [Fact]
        public void SimpleExtension()
        {            
            var input = new List<ProtoInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement(typeof (DummyClass), RootNs),
                P.Attribute<DummyClass>(d => d.SampleProperty, "{Dummy}", RootNs.Prefix),
            };

            var expectedInstructions = new List<Instruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                X.StartMember<DummyClass>(d => d.SampleProperty),
                X.StartObject(typeof(DummyExtension)),
                X.EndObject(),
                X.EndMember(),                
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            Assert.Equal(expectedInstructions, actualNodes);
        }

        [Fact]
        public void ExtensionWithOption()
        {
            var input = new List<ProtoInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement(typeof (DummyClass), RootNs),
                P.Attribute<DummyClass>(d => d.SampleProperty, "{Dummy Option}", RootNs.Prefix),
            };

            var expectedInstructions = new List<Instruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof (DummyClass)),
                X.StartMember<DummyClass>(d => d.SampleProperty),
                X.StartObject(typeof (DummyExtension)),
                X.MarkupExtensionArguments(),
                X.Value("Option"),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            Assert.Equal(expectedInstructions, actualNodes);
        }
    }
}
