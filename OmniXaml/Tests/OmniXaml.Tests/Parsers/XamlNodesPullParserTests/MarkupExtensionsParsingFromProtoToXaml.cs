namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System.Collections.Generic;
    using System.Linq;
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
                p.NamespacePrefixDeclaration("root", string.Empty),
                p.EmptyElement(typeof (DummyClass), "root"),
                p.Attribute<DummyClass>(d => d.SampleProperty, "{Dummy}"),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""),
                x.StartObject(typeof(DummyClass)),
                x.StartMember<DummyClass>(d => d.SampleProperty),
                x.StartObject(typeof(DummyExtension)),
                x.EndObject(),
                x.EndMember(),
                //x.None(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }
    }
}
