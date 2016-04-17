namespace OmniXaml.Tests.Parsers.InstructionParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Classes;
    using Common.DotNetFx;
    using Xunit;
    using OmniXaml.Parsers.Parser;
    using Resources;

    public class ParsingTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {
        private readonly IInstructionParser sut;
        private readonly ProtoInstructionResources protoResources;
        private readonly InstructionResources source;

        public ParsingTests()
        {
            sut = CreateSut();
            protoResources = new ProtoInstructionResources(this);
            source = new InstructionResources(this);
        }

        private InstructionParser CreateSut()
        {
            return new InstructionParser(RuntimeTypeSource);
        }

        [Fact]
        public void NamespaceDeclarationOnly()
        {
            var input = new List<ProtoInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
            };

            var expectedInstructions = new List<Instruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
            };


            var actualNodes = sut.Parse(input);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void SingleInstanceCollapsed()
        {
            var input = protoResources.SingleCollapsed;

            var expectedInstructions = source.OneObject;

            var actualNodes = sut.Parse(input);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void SingleOpenAndClose()
        {
            var input = protoResources.SingleOpenAndClose;

            var expectedInstructions = source.OneObject;

            var actualNodes = sut.Parse(input);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void EmptyElementWithStringProperty()
        {
            var input = protoResources.EmptyElementWithStringProperty;

            var actualNodes = sut.Parse(input);

            Assert.Equal(source.ObjectWithMember.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void EmptyElementWithTwoStringProperties()
        {
            var input = protoResources.EmptyElementWithTwoStringProperties;

            var actualNodes = sut.Parse(input);

            Assert.Equal(source.ObjectWithTwoMembers.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ElementWith2NsDeclarations()
        {
            var input = protoResources.ElementWith2NsDeclarations2;

            var actualNodes = sut.Parse(input);

            Assert.Equal(source.ElementWithTwoDeclarations.ToList(), actualNodes.ToList());
        }



        [Fact]
        public void ElementWithNestedChild()
        {
            var input = protoResources.ElementWithNestedChild;

            var actualNodes = sut.Parse(input);

            Assert.Equal(source.NestedChild.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ComplexNesting()
        {
            var input = protoResources.ComplexNesting;

            var actualNodes = sut.Parse(input).ToList();

            Assert.Equal(source.ComplexNesting.ToList(), actualNodes);
        }

        [Fact]
        public void ChildCollection()
        {
            var input = protoResources.CollectionWithMoreThanOneItem;
            var actualNodes = sut.Parse(input).ToList();

            Assert.Equal(source.CollectionWithMoreThanOneItem.ToList(), actualNodes);
        }


        [Fact]
        public void NestedChildWithContentProperty()
        {

            var input = protoResources.NestedChildWithContentProperty;

            var actual = sut.Parse(input).ToList();

            var expected = source.NestedChildWithContentProperty.ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NestedCollectionWithContentProperty()
        {
            var input = protoResources.NestedCollectionWithContentProperty;

            var actualNodes = sut.Parse(input).ToList();

            Assert.Equal(source.CollectionWithMoreThanOneItem.ToList(), actualNodes);
        }

        [Fact]
        public void CollectionsContentPropertyNesting()
        {
            var input = protoResources.ContentPropertyNesting;
            var actualNodes = sut.Parse(input).ToList();
            var expectedInstructions = source.ContentPropertyNesting;

            Assert.Equal(expectedInstructions.ToList(), actualNodes);
        }

        [Fact]
        public void TwoNestedProperties()
        {
            var input = protoResources.TwoNestedProperties;
            var actualNodes = sut.Parse(input).ToList();
            var expectedInstructions = source.TwoNestedProperties;

            Assert.Equal(expectedInstructions.ToList(), actualNodes);
        }

        [Fact]
        public void TwoNestedPropertiesUsingContentProperty()
        {
            var input = protoResources.TwoNestedPropertiesUsingContentProperty;

            var actualNodes = sut.Parse(input).ToList();

            Assert.Equal(source.TwoNestedPropertiesUsingContentProperty.ToList(), actualNodes);
        }

        [Fact]
        public void TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem()
        {

            var input = protoResources.TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem;

            var actualNodes = sut.Parse(input).ToList();

            Assert.Equal(source.TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem.ToList(), actualNodes);
        }

        [Fact]
        public void MixedPropertiesWithContentPropertyAfter()
        {
            Assert.Equal(
                source.MixedPropertiesWithContentPropertyAfter.ToList(),
                sut.Parse(protoResources.MixedPropertiesWithContentPropertyAfter).ToList());
        }

        [Fact]
        public void CollectionWithMixedEmptyAndNotEmptyNestedElements()
        {
            Assert.Equal(
                source.CollectionWithMixedEmptyAndNotEmptyNestedElements.ToList(),
                sut.Parse(protoResources.CollectionWithMixedEmptyAndNotEmptyNestedElements).ToList());
        }

        [Fact]
        public void MixedPropertiesWithContentPropertyBefore()
        {
            var input = protoResources.MixedPropertiesWithContentPropertyBefore;

            var actualNodes = sut.Parse(input).ToList();

            Assert.Equal(source.MixedPropertiesWithContentPropertyBefore.ToList(), actualNodes);
        }

        [Fact]
        public void ImplicitContentPropertyWithImplicityCollection()
        {
            var input = protoResources.ImplicitContentPropertyWithImplicityCollection;

            var actualNodes = sut.Parse(input).ToList();

            Assert.Equal(source.CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection().ToList(), actualNodes);
        }

        [Fact]
        public void ClrNamespace()
        {
            var type = typeof(DummyClass);
            string clrNamespace = $"clr-namespace:{type.Namespace};Assembly={type.GetTypeInfo().Assembly.GetName().Name}";
            var prefix = "prefix";
            var input = new List<ProtoInstruction>
            {
                P.NamespacePrefixDeclaration(prefix, clrNamespace),
                P.EmptyElement(type, RootNs),
            };

            var expectedInstructions = new List<Instruction>
            {
                X.NamespacePrefixDeclaration(clrNamespace, prefix),
                X.StartObject<DummyClass>(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ExpandedStringProperty()
        {
            var input = protoResources.ExpandedStringProperty;

            var expectedInstructions = source.ExpandedStringProperty;

            var actualNodes = sut.Parse(input);
            var xamlNodes = actualNodes.ToList();

            Assert.Equal(expectedInstructions.ToList(), xamlNodes);
        }

        [Fact]
        public void TextInInnerContent()
        {
            var actual = sut.Parse(protoResources.ContentPropertyInInnerContent).ToList();
            var expected = source.TextBlockWithText.ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");
            var input = protoResources.GetString(sysNs);

            var expectedInstructions = source.GetString(sysNs);

            var actualNodes = sut.Parse(input);
            var xamlNodes = actualNodes.ToList();

            Assert.Equal(expectedInstructions.ToList(), xamlNodes);
        }

        [Fact]
        public void PureCollection()
        {
            var actual = sut.Parse(protoResources.PureCollection).ToList();
            var expected = source.PureCollection.ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MixedCollection()
        {
            var actual = sut.Parse(protoResources.MixedCollection).ToList();
            var expected = source.MixedCollection.ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ChildInDeeperNameScopeWithNamesInTwoLevels()
        {
            var actual = sut.Parse(protoResources.ChildInDeeperNameScopeWithNamesInTwoLevels).ToList();
            var expected = source.ListBoxWithItemAndTextBlockWithNames.ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DirectContentForOneToMany()
        {
            var expected = source.DirectContentForOneToMany.ToList();
            var actual = sut.Parse(protoResources.DirectContentForOneToMany).ToList();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ImplicitCollection()
        {
            var sut = CreateSut();
            var expected = source.ImplicitCollection.ToList();
            var actual = sut.Parse(protoResources.ImplicitCollection).ToList();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExplicitCollection()
        {
            var sut = CreateSut();
            var expected = source.ExplicitCollection.ToList();
            var actual = sut.Parse(protoResources.ExplicitCollection).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AttachableMemberThatIsCollection()
        {
            var sut = CreateSut();
            var expected = source.AttachableMemberThatIsCollection.ToList();
            var actual = sut.Parse(protoResources.AttachableMemberThatIsCollection).ToList();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AttachableMemberThatIsCollectionImplicit()
        {
            var sut = CreateSut();
            var expected = source.AttachableMemberThatIsCollectionImplicit.ToList();
            var actual = sut.Parse(protoResources.AttachableMemberThatIsCollectionImplicit).ToList();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExpandedAttachablePropertyAndItemBelow()
        {
            var actual = sut.Parse(protoResources.ExpandedAttachablePropertyAndItemBelow).ToList();
            var expected = source.ExpandedAttachablePropertyAndItemBelow.ToList();
            Assert.Equal(expected, actual);
        }
    }
}
