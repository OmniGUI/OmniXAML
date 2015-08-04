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
            builder = new ProtoNodeBuilder(WiringContext.TypeContext, WiringContext.FeatureProvider);
            sut = new ProtoParser(WiringContext.TypeContext);
        }

        [TestMethod]
        public void SingleCollapsed()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleCollapsed).ToList();
            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration(RootNs),
                builder.EmptyElement<DummyClass>(RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass), RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass), RootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                builder.EmptyElement<ChildClass>(RootNs),
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
                builder.EmptyElement(typeof(DummyClass), RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NamespacePrefixDeclaration("a", "another"),
                builder.EmptyElement(typeof(DummyClass), RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass), RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass), RootNs),
                builder.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs),
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
                builder.NonEmptyElement(typeof(DummyClass),  RootNs),
                builder.AttachableProperty<Container>("Property", "Value", RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass),  RootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                builder.NonEmptyElement(typeof(ChildClass), RootNs),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, RootNs),
                builder.EmptyElement(typeof(ChildClass), RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass), RootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                builder.NonEmptyElement(typeof(ChildClass), RootNs),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, RootNs),
                builder.NonEmptyElement(typeof(ChildClass), RootNs),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, RootNs),
                builder.EmptyElement(typeof(ChildClass), RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass), RootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                builder.EmptyElement(typeof(Item), RootNs),
                builder.Text(),
                builder.EmptyElement(typeof(Item), RootNs),
                builder.Text(),
                builder.EmptyElement(typeof(Item), RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass), RootNs),
                builder.EmptyElement(typeof(Item), RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.EmptyElement(typeof(DummyClass), RootNs),                
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass), RootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                builder.EndTag(),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass), RootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                builder.EmptyElement<Item>(RootNs),
                builder.Attribute<Item>(i => i.Title, "Main1", RootNs),
                builder.Text(),
                builder.EmptyElement<Item>(RootNs),
                builder.Attribute<Item>(i => i.Title, "Main2", RootNs),
                builder.Text(),
                builder.EndTag(),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                builder.NonEmptyElement(typeof(ChildClass),  RootNs),
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
                builder.NamespacePrefixDeclaration(RootNs),
                builder.NonEmptyElement(typeof(DummyClass), RootNs),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.SampleProperty, RootNs),
                                         builder.Text("Property!"),
                                         builder.EndTag(),
                                         builder.EndTag(),
                                         builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedNodes, actualNodes);
        }
    }
}