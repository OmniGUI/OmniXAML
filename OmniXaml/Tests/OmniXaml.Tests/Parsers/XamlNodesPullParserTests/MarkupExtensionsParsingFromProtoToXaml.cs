namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Builder;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.XamlNodes;

    [TestClass]
    public class MarkupExtensionsParsingFromProtoToXaml : GivenAWiringContextWithNodeBuilders
    {
        private IXamlNodesPullParser sut;

        
        public MarkupExtensionsParsingFromProtoToXaml()
        {            
            sut = new XamlNodesPullParser(WiringContext);
        }

        [TestMethod]
        public void SimpleExtension()
        {
            

            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement(typeof (DummyClass), RootNs),
                P.Attribute<DummyClass>(d => d.SampleProperty, "{Dummy}", RootNs),
            };

            var expectedNodes = new List<XamlNode>
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

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ExtensionWithOption()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement(typeof (DummyClass), RootNs),
                P.Attribute<DummyClass>(d => d.SampleProperty, "{Dummy Option}", RootNs),
            };

            var expectedNodes = new List<XamlNode>
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

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }
    }
}
