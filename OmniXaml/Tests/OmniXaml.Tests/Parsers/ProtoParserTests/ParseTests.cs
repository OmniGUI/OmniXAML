namespace OmniXaml.Tests.Parsers.ProtoParserTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers;
    using OmniXaml.Parsers.ProtoParser;
    using Xaml.Tests.Resources;

    [TestClass]
    public class ParseTests : GivenAWiringContext
    {
        private readonly ProtoNodeBuilder builder;
        private readonly ProtoParser sut;

        public ParseTests()
        {
            builder = new ProtoNodeBuilder(WiringContext.TypeContext);
            sut = new ProtoParser(WiringContext.TypeContext);
        }

        [TestMethod]
        public void SingleCollapsed()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleCollapsed).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.EmptyElement<DummyClass>(),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void SingleOpenAndClose()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleOpenAndClose).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NonEmptyElement(typeof(DummyClass)),
                builder.EndTag(),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ElementWithChild()
        {
            var actualNodes = sut.Parse(ProtoInputs.ElementWithChild).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NonEmptyElement(typeof(DummyClass)),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child),
                builder.EmptyElement<ChildClass>(),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void SingleCollapsedWithNs()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleCollapsedWithNs).ToList();
            const string oneNamespace = "root";

            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", oneNamespace),
                builder.EmptyElement(typeof(DummyClass), ""),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            var actualNodes = sut.Parse(ProtoInputs.ElementWith2NsDeclarations).ToList();

            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", "root"),
                builder.NamespacePrefixDeclaration("a", "another"),
                builder.EmptyElement(typeof(DummyClass), ""),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void SingleOpenWithNs()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleOpenAndCloseWithNs).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", "root"),
                builder.NonEmptyElement(typeof(DummyClass), string.Empty),
                builder.EndTag(),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        [ExpectedException(typeof(XamlParseException))]
        public void PropertyTagOpen()
        {
            var actualNodes = sut.Parse(ProtoInputs.PropertyTagOpen).ToList();
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var actualNodes = sut.Parse(Dummy.StringProperty).ToList();

            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", "root"),
                builder.NonEmptyElement(typeof(DummyClass), string.Empty),
                builder.Attribute<DummyClass>(d => d.SampleProperty, "Property!", ""),
                builder.EndTag(),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void AttachedProperty()
        {
            var actualNodes = sut.Parse(Dummy.WithAttachableProperty).ToList();
            var prefix = "root";
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", prefix),
                builder.NonEmptyElement(typeof(DummyClass),  string.Empty),
                builder.AttachableProperty<Container>("Property", "Value", ""),
                builder.EndTag(),
                builder.None()
            };


            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ThreeLevelsOfNesting()
        {
            var actualNodes = sut.Parse(Dummy.ThreeLevelsOfNesting).ToList();

            var root = "root";
            ICollection expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", "root"),
                builder.NonEmptyElement(typeof(DummyClass),  string.Empty),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, ""),
                builder.NonEmptyElement(typeof(ChildClass), string.Empty),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, ""),
                builder.EmptyElement(typeof(ChildClass), ""),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.None(),
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
                builder.NamespacePrefixDeclaration("", "root"),
                builder.NonEmptyElement(typeof(DummyClass), string.Empty),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, string.Empty),
                builder.NonEmptyElement(typeof(ChildClass), string.Empty),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, string.Empty),
                builder.NonEmptyElement(typeof(ChildClass),  string.Empty),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, string.Empty),
                builder.EmptyElement(typeof(ChildClass), string.Empty),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.None(),
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
                builder.NamespacePrefixDeclaration("", "root"),
                builder.NonEmptyElement(typeof(DummyClass), string.Empty),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, string.Empty),
                builder.EmptyElement(typeof(Item), string.Empty),
                builder.Text(),
                builder.EmptyElement(typeof(Item), string.Empty),
                builder.Text(),
                builder.EmptyElement(typeof(Item), string.Empty),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),                
                builder.None(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionOneElement()
        {
            var root = "root";

            var actualNodes = sut.Parse(Dummy.ContentPropertyForCollectionOneElement).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", "root"),
                builder.NonEmptyElement(typeof(DummyClass), string.Empty),
                builder.EmptyElement(typeof(Item), ""),
                builder.Text(),
                builder.EndTag(),
                builder.None(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollapsedTag()
        {
            var root = "root";

            var actualNodes = sut.Parse(Dummy.CollapsedTag).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", "root"),
                builder.EmptyElement(typeof(DummyClass), ""),                
                builder.None(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesEmpty()
        {
            var root = "root";

            var actualNodes = sut.Parse(Dummy.TwoNestedPropertiesEmpty).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", "root"),
                builder.NonEmptyElement(typeof(DummyClass), string.Empty),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, ""),
                builder.EndTag(),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, ""),
                builder.EndTag(),
                builder.EndTag(),
                builder.None(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void TwoNestedProperties()
        {
            var actualNodes = sut.Parse(Dummy.TwoNestedProperties).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", "root"),
                builder.NonEmptyElement(typeof(DummyClass), string.Empty),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, ""),
                builder.EmptyElement<Item>(""),
                builder.Attribute<Item>(i => i.Title, "Main1", ""),
                builder.Text(),
                builder.EmptyElement<Item>(""),
                builder.Attribute<Item>(i => i.Title, "Main2", ""),
                builder.Text(),
                builder.EndTag(),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, ""),
                builder.NonEmptyElement(typeof(ChildClass),  string.Empty),
                builder.EndTag(),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.None(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }
    }
}