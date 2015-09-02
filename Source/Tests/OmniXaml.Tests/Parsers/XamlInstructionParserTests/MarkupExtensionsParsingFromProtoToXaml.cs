namespace OmniXaml.Tests.Parsers.XamlInstructionParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.XamlInstructions;

    [TestClass]
    public class MarkupExtensionsParsingFromProtoToXaml : GivenAWiringContextWithNodeBuildersNetCore
    {
        private readonly IXamlInstructionParser sut;
        
        public MarkupExtensionsParsingFromProtoToXaml()
        {            
            sut = new XamlInstructionParser(WiringContext);
        }

        [TestMethod]
        public void SimpleExtension()
        {            
            var input = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement(typeof (DummyClass), RootNs),
                P.Attribute<DummyClass>(d => d.SampleProperty, "{Dummy}", RootNs.Prefix),
            };

            var expectedInstructions = new List<XamlInstruction>
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

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ExtensionWithOption()
        {
            var input = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement(typeof (DummyClass), RootNs),
                P.Attribute<DummyClass>(d => d.SampleProperty, "{Dummy Option}", RootNs.Prefix),
            };

            var expectedInstructions = new List<XamlInstruction>
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

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }
    }
}
