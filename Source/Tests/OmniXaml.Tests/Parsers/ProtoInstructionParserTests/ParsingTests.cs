namespace OmniXaml.Tests.Parsers.ProtoInstructionParserTests
{
    using System.Linq;
    using Common.DotNetFx;
    using OmniXaml.Parsers.ProtoParser;
    using Resources;
    using Xaml.Tests.Resources;
    using Xunit;

    public class ParsingTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {
        private readonly ProtoInstructionResources source;

        public ParsingTests()
        {            
            source = new ProtoInstructionResources(this);
        }

        private ProtoInstructionParser CreateSut()
        {
            
            return new ProtoInstructionParser(RuntimeTypeSource);
        }

        [Fact]
        public void SingleCollapsed()
        {
            var sut = CreateSut();
            Assert.Equal(source.SingleCollapsed.ToList(), sut.Parse(ProtoInputs.SingleCollapsed).ToList());
        }

        [Fact]
        public void SingleOpenAndClose()
        {
            var sut = CreateSut();
            Assert.Equal(source.SingleOpenAndClose.ToList(), sut.Parse(ProtoInputs.SingleOpenAndClose).ToList());
        }

        [Fact]
        public void ElementWithChild()
        {
            var sut = CreateSut();
            Assert.Equal(source.ElementWithChild.ToList(), sut.Parse(ProtoInputs.ElementWithChild).ToList());
        }


        [Fact]
        public void ElementWith2NsDeclarations()
        {
            var sut = CreateSut();
            Assert.Equal(source.ElementWith2NsDeclarations.ToList(), sut.Parse(ProtoInputs.ElementWith2NsDeclarations).ToList());
        }

        [Fact]
        public void SingleOpenWithNs()
        {
            var sut = CreateSut();
            Assert.Equal(source.SingleOpenWithNs.ToList(), sut.Parse(ProtoInputs.SingleOpenAndCloseWithNs).ToList());
        }


        [Fact]
        public void TwoNestedProperties()
        {
            var sut = CreateSut();
            Assert.Equal(source.TwoNestedProperties.ToList(), sut.Parse(Dummy.TwoNestedProperties).ToList());
        }

        [Fact]        
        public void PropertyTagOpen()
        {
            var sut = CreateSut();

            Assert.Throws<ParseException>(() => sut.Parse(ProtoInputs.PropertyTagOpen).ToList());
        }

        [Fact]
        public void AttachedProperty()
        {
            var sut = CreateSut();
            Assert.Equal(source.AttachedProperty.ToList(), sut.Parse(Dummy.WithAttachableProperty).ToList());
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

        [Fact]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var sut = CreateSut();
            Assert.Equal(source.InstanceWithStringPropertyAndNsDeclaration.ToList(), sut.Parse(Dummy.StringProperty).ToList());
        }

        [Fact]
        public void KeyDirective()
        {
            var sut = CreateSut();
            Assert.Equal(source.KeyDirective.ToList(), sut.Parse(Dummy.KeyDirective).ToList());
        }

        [Fact]
        public void ContentPropertyForCollectionOneElement()
        {
            var sut = CreateSut();
            Assert.Equal(source.ContentPropertyForCollectionOneElement.ToList(), sut.Parse(Dummy.ContentPropertyForCollectionOneElement).ToList());
        }

        [Fact]
        public void ThreeLevelsOfNesting()
        {
            var sut = CreateSut();
            var actualNodes = sut.Parse(Dummy.ThreeLevelsOfNesting).ToList();

            Assert.Equal(source.ThreeLevelsOfNesting.ToList(), actualNodes);
        }

        [Fact]
        public void String()
        {
            var sut = CreateSut();
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            Assert.Equal(source.GetString(sysNs).ToList(), sut.Parse(Dummy.String).ToList());
        }

        [Fact]
        public void FourLevelsOfNesting()
        {
            var sut = CreateSut();
            Assert.Equal(source.FourLevelsOfNesting.ToList(), sut.Parse(Dummy.FourLevelsOfNesting).ToList());
        }

        [Fact]
        public void CollapsedTag()
        {
            var sut = CreateSut();
            Assert.Equal(source.CollapsedTag.ToList(), sut.Parse(Dummy.CollapsedTag).ToList());
        }

        [Fact]
        public void ChildCollection()
        {
            var sut = CreateSut();
            Assert.Equal(source.CollectionWithMoreThanOneItem.ToList(), sut.Parse(Dummy.ChildCollection).ToList());
        }

        [Fact]
        public void ExpandedStringProperty()
        {
            var sut = CreateSut();
            Assert.Equal(source.ExpandedStringProperty.ToList(), sut.Parse(Dummy.InnerContent).ToList());
        }

        [Fact]
        public void TwoNestedPropertiesEmpty()
        {
            var sut = CreateSut();
            Assert.Equal(source.TwoNestedPropertiesEmpty.ToList(), sut.Parse(Dummy.TwoNestedPropertiesEmpty).ToList());
        }

        [Fact]
        public void CommentIsIgnored()
        {
            var sut = CreateSut();
            Assert.Equal(source.SingleOpenAndClose.ToList(), sut.Parse(Dummy.Comment).ToList());
        }

        [Fact]
        public void TextInsideTextBlockIsAssignedAsTextProperty()
        {
            var sut = CreateSut();
            Assert.Equal(source.ContentPropertyInInnerContent.ToList(), sut.Parse(Dummy.ContentPropertyInInnerContent).ToList());
        }

        [Fact]
        public void NonExistingProperty()
        {
            var sut = CreateSut();
            Assert.Throws<ParseException>(() => sut.Parse(Dummy.NonExistingProperty).ToList());
        }

        [Fact]
        public void PureCollection()
        {
            var sut = CreateSut();
            var actual = sut.Parse(Dummy.PureCollection).ToList();
            var expected = source.PureCollection.ToList();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MixedCollection()
        {
            var sut = CreateSut();
            var actual = sut.Parse(Dummy.MixedCollection).ToList();
            var expected = source.MixedCollection.ToList();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ChildInDeeperNameScopeWithNamesInTwoLevels()
        {
            var sut = CreateSut();
            var expected = source.ChildInDeeperNameScopeWithNamesInTwoLevels.ToList();
            var actual = sut.Parse(Dummy.ChildInDeeperNameScopeWithNamesInTwoLevels).ToList();
            Assert.Equal(expected, actual);
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
