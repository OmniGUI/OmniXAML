namespace OmniXaml.Tests.Parsers.ProtoParserTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Classes.Another;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers;
    using OmniXaml.Parsers.ProtoParser;
    using Xaml.Tests.Resources;

    [TestClass]
    public class PrefixTests : GivenAWiringContext
    {
        private readonly ProtoNodeBuilder builder;
        private readonly ProtoParser sut;

        public PrefixTests()
        {
            builder = new ProtoNodeBuilder(WiringContext.TypeContext);
            sut = new ProtoParser(WiringContext.TypeContext);
        }

        [TestMethod]
        public void SingleCollapsed()
        {
            var actualNodes = sut.Parse("<x:Foreigner xmlns:x=\"another\"/>").ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("x", "another"),
                builder.EmptyElement(typeof (Foreigner), "x"),
                builder.None()
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
                builder.None()
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }
    }
}