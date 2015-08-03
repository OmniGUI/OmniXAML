namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Builder;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.XamlNodes;

    [TestClass]
    public class MarkupExtensionsParsingFromProtoToXaml : GivenAWiringContext
    {
        private XamlNodesPullParser sut;
        private ProtoNodeBuilder p;
        private readonly XamlNodeBuilder x;
        
        public MarkupExtensionsParsingFromProtoToXaml()
        {
            p = new ProtoNodeBuilder(WiringContext.TypeContext);
            x = new XamlNodeBuilder(WiringContext.TypeContext);
            sut = new XamlNodesPullParser(WiringContext);
        }

        [TestMethod]
        public void SimpleExtension()
        {
            

            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.EmptyElement(typeof (DummyClass), ""),
                p.Attribute<DummyClass>(d => d.SampleProperty, "{Dummy}", ""),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof(DummyClass)),
                x.StartMember<DummyClass>(d => d.SampleProperty),
                x.StartObject(typeof(DummyExtension)),
                x.EndObject(),
                x.EndMember(),                
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ExtensionWithOption()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.EmptyElement(typeof (DummyClass), ""),
                p.Attribute<DummyClass>(d => d.SampleProperty, "{Dummy Option}", ""),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof (DummyClass)),
                x.StartMember<DummyClass>(d => d.SampleProperty),
                x.StartObject(typeof (DummyExtension)),
                x.MarkupExtensionArguments(),
                x.Value("Option"),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }
    }
}
