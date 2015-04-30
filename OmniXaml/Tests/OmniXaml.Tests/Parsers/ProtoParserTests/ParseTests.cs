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
            var actualStates = sut.Parse(ProtoInputs.SingleCollapsed).ToList();
            var expectedStates = new List<ProtoXamlNode>
            {
                builder.EmptyElement(typeof(DummyClass), string.Empty),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void SingleOpenAndClose()
        {
            var actualStates = sut.Parse(ProtoInputs.SingleOpenAndClose).ToList();
            var expectedStates = new List<ProtoXamlNode>
            {
                builder.NonEmptyElement(typeof(DummyClass), string.Empty),
                builder.EndTag(),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void PropertyTagOpenWithInstance()
        {
            var actualStates = sut.Parse(ProtoInputs.ElementWithChild).ToList();
            var expectedStates = new List<ProtoXamlNode>
            {
                builder.NonEmptyElement(typeof(DummyClass), string.Empty),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, string.Empty),
                builder.EmptyElement(typeof(ChildClass), string.Empty),
                builder.Text(),
                builder.EndTag(),
                builder.EndTag(),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void SingleCollapsedWithNs()
        {
            var actualStates = sut.Parse(ProtoInputs.SingleCollapsedWithNs).ToList();
            const string oneNamespace = "root";

            var expectedStates = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration(oneNamespace, ""),
                builder.EmptyElement(typeof(DummyClass), oneNamespace),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            var actualStates = sut.Parse(ProtoInputs.ElementWith2NsDeclarations).ToList();

            var expectedStates = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NamespacePrefixDeclaration("another", "a"),
                builder.EmptyElement(typeof(DummyClass), "root"),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void SingleOpenWithNs()
        {
            var actualStates = sut.Parse(ProtoInputs.SingleOpenAndCloseWithNs).ToList();
            var expectedStates = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), "root"),
                builder.EndTag(),
                builder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        [ExpectedException(typeof(XamlParseException))]
        public void PropertyTagOpen()
        {
            var actualStates = sut.Parse(ProtoInputs.PropertyTagOpen).ToList();
        }

        [TestMethod]
        public void AttachedProperty()
        {
            var actualStates = sut.Parse(Dummy.WithAttachableProperty).ToList();
            var expectedStates = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), "root"),
                builder.AttachableProperty<Container>("Property"),
                builder.EndTag(),
                builder.None()
            };


            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var actualNodes = sut.Parse(Dummy.StringProperty).ToList();

            var expectedNodes = new List<ProtoXamlNode>
            {
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), "root"),
                builder.AttachableProperty<Container>("Property"),
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
                builder.NamespacePrefixDeclaration("root", ""),
                builder.NonEmptyElement(typeof(DummyClass), root),
                builder.NonEmptyPropertyElement<DummyClass>(d => d.Child, root),
                builder.NonEmptyElement(typeof(ChildClass), root),
                builder.NonEmptyPropertyElement<ChildClass>(d => d.Child, root),
                builder.EmptyElement(typeof(ChildClass), root),
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
    }
}