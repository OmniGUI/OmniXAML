namespace OmniXaml.Tests.Parsers.InstructionParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Classes;
    using Common.DotNetFx;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.Parser;
    using Resources;
    using Xunit;

    [TestClass]
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

        [TestMethod]
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

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void SingleInstanceCollapsed()
        {
            var input = protoResources.SingleCollapsed;

            var expectedInstructions = source.OneObject;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void SingleOpenAndClose()
        {
            var input = protoResources.SingleOpenAndClose;

            var expectedInstructions = source.OneObject;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void EmptyElementWithStringProperty()
        {
            var input = protoResources.EmptyElementWithStringProperty;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(source.ObjectWithMember.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void EmptyElementWithTwoStringProperties()
        {
            var input = protoResources.EmptyElementWithTwoStringProperties;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(source.ObjectWithTwoMembers.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            var input = protoResources.ElementWith2NsDeclarations2;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(source.ElementWithTwoDeclarations.ToList(), actualNodes.ToList());
        }



        [TestMethod]
        public void ElementWithNestedChild()
        {
            var input = protoResources.ElementWithNestedChild;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(source.NestedChild.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void ComplexNesting()
        {
            var input = protoResources.ComplexNesting;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.ComplexNesting.ToList(), actualNodes);
        }

        [TestMethod]
        public void ChildCollection()
        {
            var input = protoResources.CollectionWithMoreThanOneItem;
            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.CollectionWithMoreThanOneItem.ToList(), actualNodes);
        }


        [TestMethod]
        public void NestedChildWithContentProperty()
        {

            var input = protoResources.NestedChildWithContentProperty;

            var actual = sut.Parse(input).ToList();

            var expected = source.NestedChildWithContentProperty.ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NestedCollectionWithContentProperty()
        {
            var input = protoResources.NestedCollectionWithContentProperty;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.CollectionWithMoreThanOneItem.ToList(), actualNodes);
        }

        [TestMethod]
        public void CollectionsContentPropertyNesting()
        {
            var input = protoResources.ContentPropertyNesting;
            var actualNodes = sut.Parse(input).ToList();
            var expectedInstructions = source.ContentPropertyNesting;

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes);
        }

        [TestMethod]
        public void TwoNestedProperties()
        {
            var input = protoResources.TwoNestedProperties;
            var actualNodes = sut.Parse(input).ToList();
            var expectedInstructions = source.TwoNestedProperties;

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesUsingContentProperty()
        {
            var input = protoResources.TwoNestedPropertiesUsingContentProperty;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.TwoNestedPropertiesUsingContentProperty.ToList(), actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem()
        {

            var input = protoResources.TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem.ToList(), actualNodes);
        }

        [TestMethod]
        public void MixedPropertiesWithContentPropertyAfter()
        {
            CollectionAssert.AreEqual(
                source.MixedPropertiesWithContentPropertyAfter.ToList(),
                sut.Parse(protoResources.MixedPropertiesWithContentPropertyAfter).ToList());
        }

        [TestMethod]
        public void CollectionWithMixedEmptyAndNotEmptyNestedElements()
        {
            CollectionAssert.AreEqual(
                source.CollectionWithMixedEmptyAndNotEmptyNestedElements.ToList(),
                sut.Parse(protoResources.CollectionWithMixedEmptyAndNotEmptyNestedElements).ToList());
        }

        [TestMethod]
        public void MixedPropertiesWithContentPropertyBefore()
        {
            var input = protoResources.MixedPropertiesWithContentPropertyBefore;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.MixedPropertiesWithContentPropertyBefore.ToList(), actualNodes);
        }

        [TestMethod]
        public void ImplicitContentPropertyWithImplicityCollection()
        {
            var input = protoResources.ImplicitContentPropertyWithImplicityCollection;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection().ToList(), actualNodes);
        }

        [TestMethod]
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

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void ExpandedStringProperty()
        {
            var input = protoResources.ExpandedStringProperty;

            var expectedInstructions = source.ExpandedStringProperty;

            var actualNodes = sut.Parse(input);
            var xamlNodes = actualNodes.ToList();

            CollectionAssert.AreEqual(expectedInstructions.ToList(), xamlNodes);
        }

        [TestMethod]
        public void TextInInnerContent()
        {
            var actual = sut.Parse(protoResources.ContentPropertyInInnerContent).ToList();
            var expected = source.TextBlockWithText.ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");
            var input = protoResources.GetString(sysNs);

            var expectedInstructions = source.GetString(sysNs);

            var actualNodes = sut.Parse(input);
            var xamlNodes = actualNodes.ToList();

            CollectionAssert.AreEqual(expectedInstructions.ToList(), xamlNodes);
        }

        [TestMethod]
        public void PureCollection()
        {
            var actual = sut.Parse(protoResources.PureCollection).ToList();
            var expected = source.PureCollection.ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MixedCollection()
        {
            var actual = sut.Parse(protoResources.MixedCollection).ToList();
            var expected = source.MixedCollection.ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChildInDeeperNameScopeWithNamesInTwoLevels()
        {
            var actual = sut.Parse(protoResources.ChildInDeeperNameScopeWithNamesInTwoLevels).ToList();
            var expected = source.ListBoxWithItemAndTextBlockWithNames.ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DirectContentForOneToMany()
        {
            var expected = source.DirectContentForOneToMany.ToList();
            var actual = sut.Parse(protoResources.DirectContentForOneToMany).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void ImplicitCollection()
        {
            var sut = CreateSut();
            var expected = source.ImplicitCollection.ToList();
            var actual = sut.Parse(protoResources.ImplicitCollection).ToList();

            Xunit.Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExplicitCollection()
        {
            var sut = CreateSut();
            var expected = source.ExplicitCollection.ToList();
            var actual = sut.Parse(protoResources.ExplicitCollection).ToList();
            Xunit.Assert.Equal(expected, actual);
        }

        [Fact]
        public void AttachableMemberThatIsCollection()
        {
            var sut = CreateSut();
            var expected = source.AttachableMemberThatIsCollection.ToList();
            var actual = sut.Parse(protoResources.AttachableMemberThatIsCollection).ToList();

            Xunit.Assert.Equal(expected, actual);
        }

        [Fact]
        public void AttachableMemberThatIsCollectionImplicit()
        {
            var sut = CreateSut();
            var expected = source.AttachableMemberThatIsCollectionImplicit.ToList();
            var actual = sut.Parse(protoResources.AttachableMemberThatIsCollectionImplicit).ToList();

            Xunit.Assert.Equal(expected, actual);
        }

        [TestMethod]
        public void ExpandedAttachablePropertyAndItemBelow()
        {
            var actual = sut.Parse(protoResources.ExpandedAttachablePropertyAndItemBelow).ToList();
            var expected = source.ExpandedAttachablePropertyAndItemBelow.ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
