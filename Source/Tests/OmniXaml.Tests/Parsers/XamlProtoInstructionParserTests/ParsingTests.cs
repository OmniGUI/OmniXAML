namespace OmniXaml.Tests.Parsers.XamlProtoInstructionParserTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers;
    using OmniXaml.Parsers.ProtoParser;
    using ParsingSources;
    using Xaml.Tests.Resources;

    [TestClass]
    public class ParsingTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private IParser<Stream, IEnumerable<ProtoXamlInstruction>> sut;
        private readonly ProtoInstructionPack source;

        public ParsingTests()
        {
            source = new ProtoInstructionPack(this);
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

            CollectionAssert.AreEqual(source.SingleCollapsed.ToList(),actualNodes);
        }

        [TestMethod]
        public void SingleOpenAndClose()
        {
            var actualNodes = sut.Parse(ProtoInputs.SingleOpenAndClose).ToList();

            CollectionAssert.AreEqual(source.SingleOpenAndClose.ToList(),actualNodes);
        }

        [TestMethod]
        public void ElementWithChild()
        {
            var actualNodes = sut.Parse(ProtoInputs.ElementWithChild).ToList();

            CollectionAssert.AreEqual(source.ElementWithChild.ToList(),actualNodes);
        }


        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            var actualNodes = sut.Parse(ProtoInputs.ElementWith2NsDeclarations).ToList();

            CollectionAssert.AreEqual(source.ElementWith2NsDeclarations.ToList(),actualNodes);
        }

        [TestMethod]
        public void SingleOpenWithNs()
        {
            var actualStates = sut.Parse(ProtoInputs.SingleOpenAndCloseWithNs).ToList();

            CollectionAssert.AreEqual(source.SingleOpenWithNs.ToList(), actualStates);
        }


        [TestMethod]
        public void TwoNestedProperties()
        {
            var actualNodes = sut.Parse(Dummy.TwoNestedProperties).ToList();
            CollectionAssert.AreEqual(source.TwoNestedProperties.ToList(), actualNodes);
        }

        [TestMethod]
        [ExpectedException(typeof(XamlParseException))]
        public void PropertyTagOpen()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            sut.Parse(ProtoInputs.PropertyTagOpen).ToList();
        }

        [TestMethod]
        public void AttachedProperty()
        {
            var actualNodes = sut.Parse(Dummy.WithAttachableProperty).ToList();

            var expectedInstructions = source.AttachedProperty;

            CollectionAssert.AreEqual(expectedInstructions.ToList(),actualNodes);
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var actualNodes = sut.Parse(Dummy.StringProperty).ToList();

            var expectedInstructions = source.InstanceWithStringPropertyAndNsDeclaration;

            CollectionAssert.AreEqual(expectedInstructions.ToList(),actualNodes);
        }

        [TestMethod]
        public void KeyDirective()
        {
            var actualNodes = sut.Parse(Dummy.KeyDirective).ToList();

            var expectedInstructions = source.KeyDirective;

            CollectionAssert.AreEqual(expectedInstructions.ToList(),actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionOneElement()
        {
            var actualNodes = sut.Parse(Dummy.ContentPropertyForCollectionOneElement).ToList();
            var expectedInstructions = source.ContentPropertyForCollectionOneElement;

            CollectionAssert.AreEqual(expectedInstructions.ToList(),actualNodes);
        }

        [TestMethod]
        public void ThreeLevelsOfNesting()
        {
            var actualNodes = sut.Parse(Dummy.ThreeLevelsOfNesting).ToList();

            CollectionAssert.AreEqual(source.ThreeLevelsOfNesting.ToList(),actualNodes);
        }

        [TestMethod]
        public void String()
        {
            var actualStates = sut.Parse(Dummy.String).ToList();

            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            CollectionAssert.AreEqual(source.GetString(sysNs).ToList(), actualStates);
        }

        [TestMethod]
        public void FourLevelsOfNesting()
        {
            var actualNodes = sut.Parse(Dummy.FourLevelsOfNesting).ToList();

            CollectionAssert.AreEqual(source.FourLevelsOfNesting.ToList(),actualNodes);
        }

        [TestMethod]
        public void CollapsedTag()
        {
            var actualNodes = sut.Parse(Dummy.CollapsedTag).ToList();
            var expectedInstructions = source.CollapsedTag;

            CollectionAssert.AreEqual(expectedInstructions.ToList(),actualNodes);
        }

        [TestMethod]
        public void ChildCollection()
        {
            var actualNodes = sut.Parse(Dummy.ChildCollection).ToList();

            CollectionAssert.AreEqual(source.CollectionWithMoreThanOneItem.ToList(),actualNodes);
        }

        [TestMethod]
        public void ExpandedStringProperty()
        {
            var actualNodes = sut.Parse(Dummy.InnerContent).ToList();

            CollectionAssert.AreEqual(source.ExpandedStringProperty.ToList(),actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesEmpty()
        {
            var actualNodes = sut.Parse(Dummy.TwoNestedPropertiesEmpty).ToList();

            CollectionAssert.AreEqual(source.TwoNestedPropertiesEmpty.ToList(),actualNodes);
        }
    }
}
