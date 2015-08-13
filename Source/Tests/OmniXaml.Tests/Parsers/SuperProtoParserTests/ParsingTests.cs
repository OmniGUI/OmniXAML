namespace OmniXaml.Tests.Parsers.SuperProtoParserTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Classes;
    using Common;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers;
    using OmniXaml.Parsers.ProtoParser;
    using Xaml.Tests.Resources;

    [TestClass]
    public class ParsingTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private IParser<Stream, IEnumerable<ProtoXamlInstruction>> sut;

        public ParsingTests()
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            sut = new XamlProtoInstructionParser(WiringContext);
        }

        [TestMethod]
        public void SingleCollapsed()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleCollapsed).ToList();

            ICollection expectedInstructions = new Collection<ProtoXamlInstruction>
            {
               P.NamespacePrefixDeclaration(RootNs),
               P.EmptyElement<DummyClass>(RootNs),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void SingleOpenAndClose()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleOpenAndClose).ToList();

            ICollection expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ElementWithChild()
        {
            var actualNodes = sut.Parse(ProtoInputs.ElementWithChild).ToList();

            ICollection expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                P.EmptyElement(typeof(ChildClass), RootNs),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void SingleCollapsedWithNs()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleCollapsedWithNs).ToList();

            ICollection expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement<DummyClass>(RootNs),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            var actualNodes = sut.Parse(ProtoInputs.ElementWith2NsDeclarations).ToList();

            var expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NamespacePrefixDeclaration("a", "another"),
                P.EmptyElement<DummyClass>(RootNs),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void SingleOpenWithNs()
        {
            var actualStates = sut.Parse(ProtoInputs.SingleOpenAndCloseWithNs).ToList();

            var expectedStates = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof(DummyClass),  RootNs),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedStates, actualStates);
        }

        [TestMethod]
        [ExpectedException(typeof(XamlParseException))]
        public void PropertyTagOpen()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            sut.Parse(ProtoInputs.PropertyTagOpen).ToList();
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var actualNodes = sut.Parse(Dummy.StringProperty).ToList();

            var expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void KeyDirective()
        {
            var actualNodes = sut.Parse(Dummy.KeyDirective).ToList();

            var expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NamespacePrefixDeclaration("x", "http://schemas.microsoft.com/winfx/2006/xaml"),
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Resources, RootNs),
                P.EmptyElement(typeof(ChildClass), RootNs),
                P.Key("SomeKey"),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void AttachedProperty()
        {
            var actualNodes = sut.Parse(Dummy.WithAttachableProperty).ToList();

            var prefix = "root";

            var expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration("", prefix),
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.AttachableProperty<Container>("Property", "Value", RootNs),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ThreeLevelsOfNesting()
        {
            var actualNodes = sut.Parse(Dummy.ThreeLevelsOfNesting).ToList();

            ICollection expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (DummyClass), RootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                P.NonEmptyElement(typeof (ChildClass), RootNs),
                P.NonEmptyPropertyElement<ChildClass>(d => d.Child, RootNs),
                P.EmptyElement(typeof (ChildClass), RootNs),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void FourLevelsOfNesting()
        {
            var actualNodes = sut.Parse(Dummy.FourLevelsOfNesting).ToList();

            ICollection expectedInstructions = new Collection<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                P.NonEmptyElement(typeof(ChildClass), RootNs),
                P.NonEmptyPropertyElement<ChildClass>(d => d.Child, RootNs),
                P.NonEmptyElement(typeof(ChildClass), RootNs),
                P.NonEmptyPropertyElement<ChildClass>(d => d.Child, RootNs),
                P.EmptyElement(typeof(ChildClass), RootNs),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ChildCollection()
        {
            var actualNodes = sut.Parse(Dummy.ChildCollection).ToList();
            var expectedInstructions = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                P.EmptyElement(typeof(Item), RootNs),
                P.Text(),
                P.EmptyElement(typeof(Item), RootNs),
                P.Text(),
                P.EmptyElement(typeof(Item), RootNs),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionOneElement()
        {
            var actualNodes = sut.Parse(Dummy.ContentPropertyForCollectionOneElement).ToList();
            var expectedInstructions = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.EmptyElement(typeof(Item), RootNs),
                P.Text(),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void CollapsedTag()
        {
            var actualNodes = sut.Parse(Dummy.CollapsedTag).ToList();
            var expectedInstructions = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement(typeof(DummyClass), RootNs),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesEmpty()
        {
            var actualNodes = sut.Parse(Dummy.TwoNestedPropertiesEmpty).ToList();
            var expectedInstructions = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                P.EndTag(),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                P.EndTag(),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void TwoNestedProperties()
        {
            var actualNodes = sut.Parse(Dummy.TwoNestedProperties).ToList();
            var expectedInstructions = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                P.EmptyElement<Item>(RootNs),
                P.Attribute<Item>(i => i.Title, "Main1", RootNs),
                P.Text(),
                P.EmptyElement<Item>(RootNs),
                P.Attribute<Item>(i => i.Title, "Main2", RootNs),
                P.Text(),
                P.EndTag(),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                P.NonEmptyElement(typeof(ChildClass), RootNs),
                P.EndTag(),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ExpandedStringProperty()
        {
            var actualNodes = sut.Parse(Dummy.InnerContent).ToList();

            var expectedInstructions = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (DummyClass), RootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.SampleProperty, RootNs),
                P.Text("Property!"),
                P.EndTag(),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void String()
        {
            var actualStates = sut.Parse(Dummy.String).ToList();

            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");
            var expectedStates = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(sysNs),
                P.NonEmptyElement(typeof (string), sysNs),
                P.Text("Text"),
                P.EndTag(),
            };

            CollectionAssert.AreEqual(expectedStates, actualStates);
        }
    }
}
