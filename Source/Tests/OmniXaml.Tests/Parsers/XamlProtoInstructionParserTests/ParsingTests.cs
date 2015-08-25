namespace OmniXaml.Tests.Parsers.XamlProtoInstructionParserTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers;
    using OmniXaml.Parsers.ProtoParser;
    using Resources;
    using Xaml.Tests.Resources;
    using XamlParseException = OmniXaml.XamlParseException;

    [TestClass]
    public class ParsingTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private IParser<Stream, IEnumerable<ProtoXamlInstruction>> sut;
        private readonly ProtoInstructionResources source;

        public ParsingTests()
        {
            source = new ProtoInstructionResources(this);
        }

        [TestInitialize]
        public void Initialize()
        {
            sut = new XamlProtoInstructionParser(WiringContext);
        }

        [TestMethod]
        public void SingleCollapsed()
        {
            CollectionAssert.AreEqual(source.SingleCollapsed.ToList(), sut.Parse(ProtoInputs.SingleCollapsed).ToList());
        }

        [TestMethod]
        public void SingleOpenAndClose()
        {
            CollectionAssert.AreEqual(source.SingleOpenAndClose.ToList(), sut.Parse(ProtoInputs.SingleOpenAndClose).ToList());
        }

        [TestMethod]
        public void ElementWithChild()
        {
            CollectionAssert.AreEqual(source.ElementWithChild.ToList(), sut.Parse(ProtoInputs.ElementWithChild).ToList());
        }


        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            CollectionAssert.AreEqual(source.ElementWith2NsDeclarations.ToList(), sut.Parse(ProtoInputs.ElementWith2NsDeclarations).ToList());
        }

        [TestMethod]
        public void SingleOpenWithNs()
        {
            CollectionAssert.AreEqual(source.SingleOpenWithNs.ToList(), sut.Parse(ProtoInputs.SingleOpenAndCloseWithNs).ToList());
        }


        [TestMethod]
        public void TwoNestedProperties()
        {
            CollectionAssert.AreEqual(source.TwoNestedProperties.ToList(), sut.Parse(Dummy.TwoNestedProperties).ToList());
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
            CollectionAssert.AreEqual(source.AttachedProperty.ToList(), sut.Parse(Dummy.WithAttachableProperty).ToList());
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            CollectionAssert.AreEqual(source.InstanceWithStringPropertyAndNsDeclaration.ToList(), sut.Parse(Dummy.StringProperty).ToList());
        }

        [TestMethod]
        public void KeyDirective()
        {
            CollectionAssert.AreEqual(source.KeyDirective.ToList(), sut.Parse(Dummy.KeyDirective).ToList());
        }

        [TestMethod]
        public void ContentPropertyForCollectionOneElement()
        {
            CollectionAssert.AreEqual(source.ContentPropertyForCollectionOneElement.ToList(), sut.Parse(Dummy.ContentPropertyForCollectionOneElement).ToList());
        }

        [TestMethod]
        public void ThreeLevelsOfNesting()
        {
            var actualNodes = sut.Parse(Dummy.ThreeLevelsOfNesting).ToList();

            CollectionAssert.AreEqual(source.ThreeLevelsOfNesting.ToList(), actualNodes);
        }

        [TestMethod]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            CollectionAssert.AreEqual(source.GetString(sysNs).ToList(), sut.Parse(Dummy.String).ToList());
        }

        [TestMethod]
        public void FourLevelsOfNesting()
        {
            CollectionAssert.AreEqual(source.FourLevelsOfNesting.ToList(), sut.Parse(Dummy.FourLevelsOfNesting).ToList());
        }

        [TestMethod]
        public void CollapsedTag()
        {
            CollectionAssert.AreEqual(source.CollapsedTag.ToList(), sut.Parse(Dummy.CollapsedTag).ToList());
        }

        [TestMethod]
        public void ChildCollection()
        {
            CollectionAssert.AreEqual(source.CollectionWithMoreThanOneItem.ToList(), sut.Parse(Dummy.ChildCollection).ToList());
        }

        [TestMethod]
        public void ExpandedStringProperty()
        {
            CollectionAssert.AreEqual(source.ExpandedStringProperty.ToList(), sut.Parse(Dummy.InnerContent).ToList());
        }

        [TestMethod]
        public void TwoNestedPropertiesEmpty()
        {
            CollectionAssert.AreEqual(source.TwoNestedPropertiesEmpty.ToList(), sut.Parse(Dummy.TwoNestedPropertiesEmpty).ToList());
        }

        [TestMethod]
        public void CommentIsIgnored()
        {
            CollectionAssert.AreEqual(source.SingleOpenAndClose.ToList(), sut.Parse(Dummy.Comment).ToList());
        }

        [TestMethod]
        public void TextInsideTextBlockIsAssignedAsTextProperty()
        {
            CollectionAssert.AreEqual(source.ContentPropertyInInnerContent.ToList(), sut.Parse(Dummy.ContentPropertyInInnerContent).ToList());
        }

        [TestMethod]
        [ExpectedException(typeof(XamlParseException))]
        public void NonExistingProperty()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            sut.Parse(Dummy.NonExistingProperty).ToList();
        }
    }
}
