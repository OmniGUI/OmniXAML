namespace OmniXaml.Tests.Parsers.SuperProtoParserTests
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.ProtoParser;
    using Xaml.Tests.Resources;

    [TestClass]
    public class ParsingTests : GivenAWiringContext
    {
        private readonly ProtoNodeBuilder builder;
        private SuperProtoParser sut;

        public ParsingTests()
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
            var actualNodes = sut.Parse(ProtoInputs.SingleCollapsed).ToList();

            ICollection expectedNodes = new Collection<ProtoXamlNode>
            {
               builder.EmptyElement<DummyClass>("root"),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void SingleCollapsedWithNs()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleCollapsedWithNs).ToList();

            ICollection expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", string.Empty),
                builder.EmptyElement<DummyClass>("root"),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            var actualNodes = sut.Parse(ProtoInputs.ElementWith2NsDeclarations).ToList();

            ICollection expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", string.Empty),
                builder.NamespacePrefixDeclaration("another", "a"),
                builder.EmptyElement<DummyClass>("root"),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void SingleOpenAndClose()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleOpenAndClose).ToList();

            ICollection expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NonEmptyElement(typeof(DummyClass), "root"),
                builder.EndTag(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ElementWithChild()
        {
            var actualNodes = sut.Parse(ProtoInputs.ElementWithChild).ToList();

            var root = "root";
            ICollection expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NonEmptyElement(typeof(DummyClass), root),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, root),
                builder.EmptyElement(typeof(ChildClass), root),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ThreeLevelsOfNesting()
        {
            var actualNodes = sut.Parse(Dummy.ThreeLevelsOfNesting).ToList();

            var root = "root";
            ICollection expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof (DummyClass), root),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, root),
                builder.NonEmptyElement(typeof (ChildClass), root),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, root),
                builder.EmptyElement(typeof (ChildClass), root),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void FourLevelsOfNesting()
        {
            var actualNodes = sut.Parse(Dummy.FourLevelsOfNesting).ToList();

            var root = "root";
            ICollection expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), root),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, root),
                builder.NonEmptyElement(typeof(ChildClass), root),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, root),
                builder.NonEmptyElement(typeof(ChildClass), root),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, root),
                builder.EmptyElement(typeof(ChildClass), root),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }
    }
}
