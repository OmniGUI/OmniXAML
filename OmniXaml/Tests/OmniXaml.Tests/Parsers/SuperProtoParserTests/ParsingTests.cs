namespace OmniXaml.Tests.Parsers.SuperProtoParserTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.ProtoParser.SuperProtoParser;
    using ProtoParserTests;
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

            var expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", string.Empty),
                builder.NamespacePrefixDeclaration("another", "a"),
                builder.EmptyElement<DummyClass>("root"),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void SingleOpenWithNs()
        {
            var actualStates = sut.Parse(ProtoInputs.SingleOpenAndCloseWithNs).ToList();

            var expectedStates = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), "root"),
                builder.EndTag(),
            };

            CollectionAssert.AreEqual(expectedStates, actualStates);
        }

        [TestMethod]
        [ExpectedException(typeof(XamlParseException))]
        public void PropertyTagOpen()
        {
            var actualStates = sut.Parse(ProtoInputs.PropertyTagOpen).ToList();
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var actualNodes = sut.Parse(Dummy.StringProperty).ToList();

            var expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), "root"),
                builder.Attribute<DummyClass>(d => d.SampleProperty, "Property!"),
                builder.EndTag(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void AttachedProperty()
        {
            var actualNodes = sut.Parse(Dummy.WithAttachableProperty).ToList();

            var expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), "root"),
                builder.AttachableProperty<Container>("Property", "Value"),
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

        [TestMethod]
        public void ChildCollection()
        {
            var root = "root";

            var actualNodes = sut.Parse(Dummy.ChildCollection).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), root),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, root),
                builder.EmptyElement(typeof(Item), root),
                builder.Text(),
                builder.EmptyElement(typeof(Item), root),
                builder.Text(),
                builder.EmptyElement(typeof(Item), root),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionOneElement()
        {
            var root = "root";

            var actualNodes = sut.Parse(Dummy.ContentPropertyForCollectionOneElement).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), root),
                builder.EmptyElement(typeof(Item), root),
                builder.Text(),
                builder.EndTag(),
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollapsedTag()
        {
            var root = "root";

            var actualNodes = sut.Parse(Dummy.CollapsedTag).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.EmptyElement(typeof(DummyClass), root),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]        
        public void TwoNestedProperties()
        {
            var root = "root";

            var actualNodes = sut.Parse(Dummy.TwoNestedPropertiesEmpty).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), root),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, root),
                builder.EndTag(),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, root),
                builder.EndTag(),
                builder.EndTag(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }
    }
}
