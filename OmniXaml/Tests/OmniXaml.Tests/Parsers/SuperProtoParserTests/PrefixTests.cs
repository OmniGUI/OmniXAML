namespace OmniXaml.Tests.Parsers.SuperProtoParserTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Classes.Another;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.ProtoParser.SuperProtoParser;

    [TestClass]
    public class PrefixTests : GivenAWiringContext
    {
        private readonly ProtoNodeBuilder builder;
        private SuperProtoParser sut;

        public PrefixTests()
        {
            builder = new ProtoNodeBuilder(WiringContext.TypeContext);
        }

        [TestInitialize]
        public void Initialize()
        {
            sut = new SuperProtoParser(WiringContext);
        }

        [TestMethod]
        public void SingleCollapsed()
        {
            var actualNodes = sut.Parse("<x:Foreigner xmlns:x=\"another\"/>").ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("x", "another"),
                builder.EmptyElement(typeof (Foreigner), "x"),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void AttachedProperty()
        {
            var actualNodes = sut.Parse(@"<DummyClass xmlns=""root"" xmlns:x=""another"" x:Foreigner.Property=""Value""></DummyClass>").ToList();

            var ns = "root";

            var expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", ns),
                builder.NamespacePrefixDeclaration("x", "another"),
                builder.NonEmptyElement(typeof (DummyClass), string.Empty),
                builder.AttachableProperty<Foreigner>("Property", "Value", "x"),
                builder.EndTag(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ElementWithPrefixThatIsDefinedAfterwards()
        {
            var actualNodes = sut.Parse(@"<custom:DummyClass xmlns:custom=""root""></custom:DummyClass>").ToList();

            var ns = "root";

            var expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("custom", ns),
                builder.NonEmptyElement(typeof (DummyClass), "custom"),
                builder.EndTag(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }
    }
}