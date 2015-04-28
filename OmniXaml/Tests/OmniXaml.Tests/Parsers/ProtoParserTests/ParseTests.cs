namespace OmniXaml.Tests.Parsers.ProtoParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers;
    using OmniXaml.Parsers.ProtoParser;
    using Xaml.Tests.Resources;

    [TestClass]
    public class ParseTests : GivenAWiringContext
    {
        private readonly ProtoNodeBuilder stateBuilder;
        private readonly ProtoParser sut;

        public ParseTests()
        {            
            stateBuilder = new ProtoNodeBuilder(WiringContext.TypeContext);
            sut = new ProtoParser(WiringContext.TypeContext);
        }

        [TestMethod]
        public void SingleCollapsed()
        {
            var actualStates = sut.Parse(ProtoInputs.SingleCollapsed).ToList();
            var expectedStates = new List<ProtoXamlNode>
            {
                stateBuilder.EmptyElement(typeof(DummyClass), string.Empty),
                stateBuilder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void SingleOpenAndClose()
        {
            var actualStates = sut.Parse(ProtoInputs.SingleOpenAndClose).ToList();
            var expectedStates = new List<ProtoXamlNode>
            {
                stateBuilder.NonEmptyElement(typeof(DummyClass), string.Empty),
                stateBuilder.EndTag(),
                stateBuilder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void PropertyTagOpenWithInstance()
        {
            var actualStates = sut.Parse(ProtoInputs.ElementWithChild).ToList();
            var expectedStates = new List<ProtoXamlNode>
            {
                stateBuilder.NonEmptyElement(typeof(DummyClass), string.Empty),
                stateBuilder.NonEmptyPropertyElement<DummyClass>(d => d.Child, string.Empty),
                stateBuilder.EmptyElement(typeof(ChildClass), string.Empty),
                stateBuilder.Text(),
                stateBuilder.EndTag(),
                stateBuilder.EndTag(),
                stateBuilder.None()
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
                stateBuilder.NamespacePrefixDeclaration(oneNamespace, ""),
                stateBuilder.EmptyElement(typeof(DummyClass), oneNamespace),
                stateBuilder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            var actualStates = sut.Parse(ProtoInputs.ElementWith2NsDeclarations).ToList();

            var expectedStates = new List<ProtoXamlNode>
            {
                stateBuilder.NamespacePrefixDeclaration("root", ""),
                stateBuilder.NamespacePrefixDeclaration("another", "a"),
                stateBuilder.EmptyElement(typeof(DummyClass), "root"),
                stateBuilder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void SingleOpenWithNs()
        {
            var actualStates = sut.Parse(ProtoInputs.SingleOpenAndCloseWithNs).ToList();
            var expectedStates = new List<ProtoXamlNode>
            {
                stateBuilder.NamespacePrefixDeclaration("root", ""),
                stateBuilder.NonEmptyElement(typeof(DummyClass), "root"),
                stateBuilder.EndTag(),
                stateBuilder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        [ExpectedException(typeof(XamlParseException))]
        public void PropertyTagOpen()
        {
            var actualStates  =sut.Parse(ProtoInputs.PropertyTagOpen).ToList();
        }
       
        [TestMethod]
        public void AttachedProperty()
        {
            var actualStates = sut.Parse(Dummy.WithAttachableProperty).ToList();
            var expectedStates = new List<ProtoXamlNode>
            {
                stateBuilder.NamespacePrefixDeclaration("root", ""),
                stateBuilder.NonEmptyElement(typeof(DummyClass), "root"),
                stateBuilder.AttachableProperty<Container>("Property"),
                stateBuilder.EndTag(),
                stateBuilder.None()
            };


            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var actualStates = sut.Parse(Dummy.StringProperty).ToList();

            var expectedStates = new List<ProtoXamlNode>
            {
                stateBuilder.NamespacePrefixDeclaration("root", ""),
                stateBuilder.NonEmptyElement(typeof(DummyClass), "root"),
                stateBuilder.AttachableProperty<Container>("Property"),
                stateBuilder.EndTag(),
                stateBuilder.None()
            };

            ProtoXamlNodeAssert.AreEqualWithLooseXamlTypeComparison(expectedStates, actualStates);
        }
    }
}