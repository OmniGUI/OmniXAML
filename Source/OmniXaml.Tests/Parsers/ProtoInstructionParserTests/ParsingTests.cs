namespace OmniXaml.Tests.Parsers.ProtoInstructionParserTests
{
    using System.Linq;
    using Common;
    using OmniXaml.Parsers.ProtoParser;
    using Resources;
    using Xaml.Tests.Resources;
    using Xunit;

    public class ParsingTests : GivenARuntimeTypeSource
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
            Assert.Equal(source.SingleCollapsed, sut.Parse(File.LoadAsString(@"Xaml\ProtoInputs\SingleCollapsed.xaml")));
        }

        [Fact]
        public void SingleOpenAndClose()
        {
            var sut = CreateSut();
            Assert.Equal(source.SingleOpenAndClose, sut.Parse(File.LoadAsString(@"Xaml\ProtoInputs\SingleOpenAndClose.xaml")));
        }

        [Fact]
        public void ElementWithChild()
        {
            var sut = CreateSut();
            Assert.Equal(source.ElementWithChild, sut.Parse(File.LoadAsString(@"Xaml\ProtoInputs\ElementWithChild.xaml")));
        }


        [Fact]
        public void ElementWith2NsDeclarations()
        {
            var sut = CreateSut();
            Assert.Equal(source.ElementWith2NsDeclarations, sut.Parse(File.LoadAsString(@"Xaml\ProtoInputs\ElementWith2NsDeclarations.xaml")));
        }

        [Fact]
        public void SingleOpenWithNs()
        {
            var sut = CreateSut();
            Assert.Equal(source.SingleOpenWithNs, sut.Parse(File.LoadAsString(@"Xaml\ProtoInputs\SingleOpenAndCloseWithNs.xaml")));
        }


        [Fact]
        public void TwoNestedProperties()
        {
            var sut = CreateSut();
            var load = File.LoadAsString(@"Xaml\Dummy\TwoNestedProperties.xaml");
            Assert.Equal(source.TwoNestedProperties, sut.Parse(load));
        }

        [Fact]        
        public void PropertyTagOpen()
        {
            var sut = CreateSut();

            Assert.Throws<ParseException>(() => sut.Parse(File.LoadAsString(@"Xaml\ProtoInputs\PropertyTagOpen.xaml")).ToList());
        }

        [Fact]
        public void AttachedProperty()
        {
            var sut = CreateSut();
            Assert.Equal(source.AttachedProperty, sut.Parse(File.LoadAsString(@"Xaml\Dummy\WithAttachableProperty.xaml")));
        }

        [Fact]
        public void NestedAttachedProperty()
        {
            var sut = CreateSut();
            Assert.Equal(source.ExpandedAttachedProperty, sut.Parse(File.LoadAsString(@"Xaml\Dummy\ExpandedAttachableProperty.xaml")));
        }

        [Fact]
        public void ExpandedAttachablePropertyAndItemBelow()
        {
            var sut = CreateSut();
            var expected = source.ExpandedAttachablePropertyAndItemBelow;
            var actual = sut.Parse(File.LoadAsString(@"Xaml\Dummy\ExpandedAttachablePropertyAndItemBelow.xaml"));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PrefixedExpandedAttachablePropertyAndItemBelow()
        {
            var sut = CreateSut();
            var expected = source.PrefixedExpandedAttachablePropertyAndItemBelow;
            var actual = sut.Parse(File.LoadAsString(@"Xaml\Dummy\PrefixedExpandedAttachablePropertyAndItemBelow.xaml"));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var sut = CreateSut();
            Assert.Equal(source.InstanceWithStringPropertyAndNsDeclaration, sut.Parse(File.LoadAsString(@"Xaml\Dummy\StringProperty.xaml")));
        }

        [Fact]
        public void KeyDirective()
        {
            var sut = CreateSut();
            Assert.Equal(source.KeyDirective, sut.Parse(File.LoadAsString(@"Xaml\Dummy\KeyDirective.xaml")));
        }

        [Fact]
        public void ContentPropertyForCollectionOneElement()
        {
            var sut = CreateSut();
            Assert.Equal(source.ContentPropertyForCollectionOneElement, sut.Parse(File.LoadAsString(@"Xaml\Dummy\ContentPropertyForCollectionOneElement.xaml")));
        }

        [Fact]
        public void ThreeLevelsOfNesting()
        {
            var sut = CreateSut();
            var actualNodes = sut.Parse(File.LoadAsString(@"Xaml\Dummy\ThreeLevelsOfNesting.xaml"));

            Assert.Equal(source.ThreeLevelsOfNesting, actualNodes);
        }

        [Fact]
        public void String()
        {
            var sut = CreateSut();
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            Assert.Equal(source.GetString(sysNs), sut.Parse(File.LoadAsString(@"Xaml\Dummy\String.xaml")));
        }

        [Fact]
        public void FourLevelsOfNesting()
        {
            var sut = CreateSut();
            Assert.Equal(source.FourLevelsOfNesting, sut.Parse(File.LoadAsString(@"Xaml\Dummy\FourLevelsOfNesting.xaml")));
        }

        [Fact]
        public void CollapsedTag()
        {
            var sut = CreateSut();
            Assert.Equal(source.CollapsedTag, sut.Parse(File.LoadAsString(@"Xaml\Dummy\CollapsedTag.xaml")));
        }

        [Fact]
        public void ChildCollection()
        {
            var sut = CreateSut();
            Assert.Equal(source.CollectionWithMoreThanOneItem, sut.Parse(File.LoadAsString(@"Xaml\Dummy\ChildCollection.xaml")));
        }

        [Fact]
        public void ExpandedStringProperty()
        {
            var sut = CreateSut();
            Assert.Equal(source.ExpandedStringProperty, sut.Parse(File.LoadAsString(@"Xaml\Dummy\InnerContent.xaml")));
        }

        [Fact]
        public void TwoNestedPropertiesEmpty()
        {
            var sut = CreateSut();
            Assert.Equal(source.TwoNestedPropertiesEmpty, sut.Parse(File.LoadAsString(@"Xaml\Dummy\TwoNestedPropertiesEmpty.xaml")));
        }

        [Fact]
        public void CommentIsIgnored()
        {
            var sut = CreateSut();
            Assert.Equal(source.SingleOpenAndClose, sut.Parse(File.LoadAsString(@"Xaml\Dummy\Comment.xaml")));
        }

        [Fact]
        public void TextInsideTextBlockIsAssignedAsTextProperty()
        {
            var sut = CreateSut();
            Assert.Equal(source.ContentPropertyInInnerContent, sut.Parse(File.LoadAsString(@"Xaml\Dummy\ContentPropertyInInnerContent.xaml")));
        }

        [Fact]
        public void NonExistingProperty()
        {
            var sut = CreateSut();
            Assert.Throws<ParseException>(() => sut.Parse(File.LoadAsString(@"Xaml\Dummy\NonExistingProperty.xaml")).ToList());
        }

        [Fact]
        public void PureCollection()
        {
            var sut = CreateSut();
            var actual = sut.Parse(File.LoadAsString(@"Xaml\Dummy\PureCollection.xaml"));
            var expected = source.PureCollection;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MixedCollection()
        {
            var sut = CreateSut();
            var actual = sut.Parse(File.LoadAsString(@"Xaml\Dummy\MixedCollection.xaml"));
            var expected = source.MixedCollection;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ChildInDeeperNameScopeWithNamesInTwoLevels()
        {
            var sut = CreateSut();
            var expected = source.ChildInDeeperNameScopeWithNamesInTwoLevels;
            var actual = sut.Parse(File.LoadAsString(@"Xaml\Dummy\ChildInDeeperNameScopeWithNamesInTwoLevels.xaml"));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NamePropetyAndNameDirectiveProduceSameProtoInstructions()
        {
            var sut = CreateSut();
            var expected = sut.Parse(File.LoadAsString(@"Xaml\Dummy\ChildInDeeperNameScopeWithNamesInTwoLevels.xaml")).ToList();
            var actual = sut.Parse(File.LoadAsString(@"Xaml\Dummy\ChildInDeeperNameScopeWithNamesInTwoLevelsNoNameDirectives.xaml")).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AttachableMemberThatIsCollection()
        {
            var sut = CreateSut();
            var expected = source.AttachableMemberThatIsCollection;
            var actual = sut.Parse(File.LoadAsString(@"Xaml\Dummy\AttachableMemberThatIsCollection.xaml"));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AttachableMemberThatIsCollectionImplicit()
        {
            var sut = CreateSut();
            var expected = source.AttachableMemberThatIsCollectionImplicit;
            var actual = sut.Parse(File.LoadAsString(@"Xaml\Dummy\AttachableMemberThatIsCollectionImplicit.xaml"));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DirectContentForOneToMany()
        {
            var sut = CreateSut();
            var expected = source.DirectContentForOneToMany;
            var actual = sut.Parse(File.LoadAsString(@"Xaml\Dummy\DirectContentForOneToMany.xaml"));
            Assert.Equal(expected, actual);
        }
    }
}
