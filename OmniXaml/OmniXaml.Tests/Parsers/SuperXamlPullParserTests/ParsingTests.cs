namespace OmniXaml.Tests.Parsers.SuperXamlPullParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.Xaml;

    [TestClass]
    public class ParsingTests : GivenAWiringContext
    {
        private readonly ProtoNodeBuilder protoBuilder;
        private readonly XamlNodeBuilder builder;
        private readonly SuperXamlPullParser sut;

        public ParsingTests()
        {
            protoBuilder = new ProtoNodeBuilder(WiringContext.TypeContext);
            builder = new XamlNodeBuilder(WiringContext.TypeContext);
            sut = new SuperXamlPullParser();
        }

        [TestMethod]
        public void NamespaceDeclarationOnly()
        {
            var input = new List<ProtoXamlNode>
            {
                protoBuilder.NamespacePrefixDeclaration("root", ""),
            };

            var expectedNodes = new List<XamlNode>
            {
                builder.NamespaceDeclaration("root", ""),
            };

            
            var actualNodes = sut.Parse(input);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void SingleInstanceCollapsed()
        {
            var input = new List<ProtoXamlNode>
            {
                protoBuilder.ElementCollapsed(typeof(DummyClass), string.Empty),
            };

            var expectedNodes = new List<XamlNode>
            {
                builder.StartObject<DummyClass>(),
                builder.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes.ToList());
        }
    }
}
