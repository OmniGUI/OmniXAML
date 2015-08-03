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
                builder.NamespacePrefixDeclaration(rootNs),
                builder.EmptyElement<DummyClass>(rootNs),
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
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass), rootNs),
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
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass), rootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                builder.EmptyElement<ChildClass>(rootNs),
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
                builder.EmptyElement(typeof(DummyClass), rootNs),
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
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NamespacePrefixDeclaration("a", "another"),
                builder.EmptyElement(typeof(DummyClass), rootNs),
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
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass), rootNs),
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
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass), rootNs),
                builder.Attribute<DummyClass>(d => d.SampleProperty, "Property!", rootNs),
                builder.EndTag(),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void InlineAttachedProperty()
        {
            var actualNodes = sut.Parse(Dummy.WithAttachableProperty).ToList();
            var prefix = "root";
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("", prefix),
                builder.NonEmptyElement(typeof(DummyClass),  rootNs),
                builder.AttachableProperty<Container>("Property", "Value", rootNs),
                builder.EndTag(),
                builder.None()
            };


            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ThreeLevelsOfNesting()
        {
            var actualNodes = sut.Parse(Dummy.ThreeLevelsOfNesting).ToList();

            ICollection expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass),  rootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                builder.NonEmptyElement(typeof(ChildClass), rootNs),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, rootNs),
                builder.EmptyElement(typeof(ChildClass), rootNs),
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

            ICollection expectedNodes = new Collection<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass), rootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                builder.NonEmptyElement(typeof(ChildClass), rootNs),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, rootNs),
                builder.NonEmptyElement(typeof(ChildClass), rootNs),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, rootNs),
                builder.EmptyElement(typeof(ChildClass), rootNs),
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
            var actualNodes = sut.Parse(Dummy.ChildCollection).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass), rootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, rootNs),
                builder.EmptyElement(typeof(Item), rootNs),
                builder.Text(),
                builder.EmptyElement(typeof(Item), rootNs),
                builder.Text(),
                builder.EmptyElement(typeof(Item), rootNs),
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
            var actualNodes = sut.Parse(Dummy.ContentPropertyForCollectionOneElement).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass), rootNs),
                builder.EmptyElement(typeof(Item), rootNs),
                builder.Text(),
                builder.EndTag(),
                builder.None(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollapsedTag()
        {
            var actualNodes = sut.Parse(Dummy.CollapsedTag).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration(rootNs),
                builder.EmptyElement(typeof(DummyClass), rootNs),                
                builder.None(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesEmpty()
        {
            var actualNodes = sut.Parse(Dummy.TwoNestedPropertiesEmpty).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass), rootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, rootNs),
                builder.EndTag(),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
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
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass), rootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, rootNs),
                builder.EmptyElement<Item>(rootNs),
                builder.Attribute<Item>(i => i.Title, "Main1", rootNs),
                builder.Text(),
                builder.EmptyElement<Item>(rootNs),
                builder.Attribute<Item>(i => i.Title, "Main2", rootNs),
                builder.Text(),
                builder.EndTag(),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                builder.NonEmptyElement(typeof(ChildClass),  rootNs),
                builder.EndTag(),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.None(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ExpandedStringProperty()
        {
            var actualNodes = sut.Parse(Dummy.InnerContent).ToList();

            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration(rootNs),
                builder.NonEmptyElement(typeof(DummyClass), rootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.SampleProperty, rootNs),
                                         builder.Text("Property!"),
                                         builder.EndTag(),
                                         builder.EndTag(),
                                         builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }
    }
}