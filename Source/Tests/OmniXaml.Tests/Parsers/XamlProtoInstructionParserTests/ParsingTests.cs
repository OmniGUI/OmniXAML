namespace OmniXaml.Tests.Parsers.XamlProtoInstructionParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NUnit.Framework;
    using OmniXaml.Parsers;
    using OmniXaml.Parsers.ProtoParser;
    using Resources;
    using Xaml.Tests.Resources;
    using Xunit;
    using Assert = Xunit.Assert;
    using CollectionAssert = Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert;
    using XamlParseException = XamlParseException;

    [TestClass]
    public class ParsingTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private IParser<IXmlReader, IEnumerable<ProtoXamlInstruction>> sut;
        private readonly ProtoInstructionResources source;

        public ParsingTests()
        {
            source = new ProtoInstructionResources(this);
        }

        [TestInitialize]
        [SetUp]
        public void Initialize()
        {
            sut = CreateSut();
        }

        private XamlProtoInstructionParser CreateSut()
        {
            return new XamlProtoInstructionParser(WiringContext);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.ExpectedException(typeof(XamlParseException))]
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

        [Fact]
        public void NestedAttachedProperty()
        {
            var sut = CreateSut();
            Assert.Equal(source.ExpandedAttachedProperty.ToList(), sut.Parse(Dummy.ExpandedAttachableProperty).ToList());
        }

        [Fact]
        public void ExpandedAttachablePropertyAndItemBelow()
        {
            var sut = CreateSut();
            var expected = source.ExpandedAttachablePropertyAndItemBelow.ToList();
            var actual = sut.Parse(Dummy.ExpandedAttachablePropertyAndItemBelow).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PrefixedExpandedAttachablePropertyAndItemBelow()
        {
            var sut = CreateSut();
            var expected = source.PrefixedExpandedAttachablePropertyAndItemBelow.ToList();
            var actual = sut.Parse(Dummy.PrefixedExpandedAttachablePropertyAndItemBelow).ToList();
            Assert.Equal(expected, actual);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.ExpectedException(typeof(XamlParseException))]
        public void NonExistingProperty()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            sut.Parse(Dummy.NonExistingProperty).ToList();
        }

        [TestMethod]
        public void PureCollection()
        {
            var actual = sut.Parse(Dummy.PureCollection).ToList();
            var expected = source.PureCollection.ToList();
            
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MixedCollection()
        {
            var actual = sut.Parse(Dummy.MixedCollection).ToList();
            var expected = source.MixedCollection.ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChildInDeeperNameScopeWithNamesInTwoLevels()
        {
            var expected = source.ChildInDeeperNameScopeWithNamesInTwoLevels.ToList();
            var actual = sut.Parse(Dummy.ChildInDeeperNameScopeWithNamesInTwoLevels).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void NamePropetyAndNameDirectiveProduceSameProtoInstructions()
        {
            var sut = CreateSut();
            var expected = sut.Parse(Dummy.ChildInDeeperNameScopeWithNamesInTwoLevels).ToList();
            var actual = sut.Parse(Dummy.ChildInDeeperNameScopeWithNamesInTwoLevelsNoNameDirectives).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AttachableMemberThatIsCollection()
        {
            var sut = CreateSut();
            var expected = source.AttachableMemberThatIsCollection.ToList();
            var actual = sut.Parse(Dummy.AttachableMemberThatIsCollection).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AttachableMemberThatIsCollectionImplicit()
        {
            var sut = CreateSut();
            var expected = source.AttachableMemberThatIsCollectionImplicit.ToList();
            var actual = sut.Parse(Dummy.AttachableMemberThatIsCollectionImplicit).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DirectContentForOneToMany()
        {
            var sut = CreateSut();
            var expected = source.DirectContentForOneToMany.ToList();
            var actual = sut.Parse(Dummy.DirectContentForOneToMany).ToList();
            Assert.Equal(expected, actual);
        }
    }   
}
