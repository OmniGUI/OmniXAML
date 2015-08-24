namespace OmniXaml.Tests.Parsers.XamlInstructionParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Classes;
    using Classes.WpfLikeModel;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.XamlNodes;
    using ParsingSources;

    [TestClass]
    public class ParsingTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private readonly IXamlInstructionParser sut;
        private readonly ProtoInstructionPack protoPack;
        private readonly XamlInstructionPack source;

        public ParsingTests()
        {          
            sut = new XamlInstructionParser(WiringContext);
            protoPack = new ProtoInstructionPack(this);
            source = new XamlInstructionPack(this);
        }

        [TestMethod]
        public void NamespaceDeclarationOnly()
        {
            var input = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(RootNs),
            };

            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
            };


            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void SingleInstanceCollapsed()
        {
            var input = protoPack.SingleCollapsed;

            var expectedInstructions = source.OneObject;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }
       
        [TestMethod]
        public void SingleOpenAndClose()
        {
            var input = protoPack.SingleOpenAndClose;

            var expectedInstructions = source.OneObject;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void EmptyElementWithStringProperty()
        {
            var input = protoPack.EmptyElementWithStringProperty;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(source.ObjectWithMember.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void EmptyElementWithTwoStringProperties()
        {
            var input = protoPack.EmptyElementWithTwoStringProperties;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(source.ObjectWithTwoMembers.ToList(), actualNodes.ToList());
        }       

        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            var input = protoPack.ElementWith2NsDeclarations2;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(source.ElementWithTwoDeclarations.ToList(), actualNodes.ToList());
        }



        [TestMethod]
        public void ElementWithNestedChild()
        {
            var input = protoPack.ElementWithNestedChild;

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(source.NestedChild.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void ComplexNesting()
        {
            var input = protoPack.ComplexNesting;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.ComplexNesting.ToList(), actualNodes);
        }

        [TestMethod]
        public void ChildCollection()
        {
            var input = protoPack.CollectionWithMoreThanOneItem;
            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.CollectionWithMoreThanOneItem.ToList(), actualNodes);
        }


        [TestMethod]
        public void NestedChildWithContentProperty()
        {

            var input = protoPack.NestedChildWithContentProperty;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.NestedChildWithContentProperty.ToList(), actualNodes);
        }

        [TestMethod]
        public void NestedCollectionWithContentProperty()
        {
            var input = protoPack.NestedCollectionWithContentProperty;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.CollectionWithMoreThanOneItem.ToList(), actualNodes);
        }

        [TestMethod]
        public void CollectionsContentPropertyNesting()
        {
            var input = protoPack.ContentPropertyNesting;
            var actualNodes = sut.Parse(input).ToList();
            var expectedInstructions = source.ContentPropertyNesting;

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes);
        }

        [TestMethod]
        public void TwoNestedProperties()
        {
            var input = protoPack.TwoNestedProperties;
            var actualNodes = sut.Parse(input).ToList();
            var expectedInstructions = source.TwoNestedProperties;

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesUsingContentProperty()
        {
            var input = protoPack.TwoNestedPropertiesUsingContentProperty;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.TwoNestedPropertiesUsingContentProperty.ToList(), actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem()
        {

            var input = protoPack.TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem.ToList(), actualNodes);
        }

        [TestMethod]
        public void MixedPropertiesWithContentPropertyAfter()
        {
            CollectionAssert.AreEqual(
                source.MixedPropertiesWithContentPropertyAfter.ToList(),
                sut.Parse(protoPack.MixedPropertiesWithContentPropertyAfter).ToList());
        }

        [TestMethod]
        public void CollectionWithMixedEmptyAndNotEmptyNestedElements()
        {
            CollectionAssert.AreEqual(
                source.CollectionWithMixedEmptyAndNotEmptyNestedElements.ToList(),
                sut.Parse(protoPack.CollectionWithMixedEmptyAndNotEmptyNestedElements).ToList());
        }

        [TestMethod]
        public void MixedPropertiesWithContentPropertyBefore()
        {
            var input = protoPack.MixedPropertiesWithContentPropertyBefore;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.MixedPropertiesWithContentPropertyBefore.ToList(), actualNodes);
        }

        [TestMethod]
        public void ImplicitContentPropertyWithImplicityCollection()
        {
            var input = protoPack.ImplicitContentPropertyWithImplicityCollection;

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(source.CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection().ToList(), actualNodes);
        }

        [TestMethod]
        public void ClrNamespace()
        {
            var type = typeof(DummyClass);
            string clrNamespace = $"clr-namespace:{type.Namespace};Assembly={type.GetTypeInfo().Assembly.GetName().Name}";
            var prefix = "prefix";
            var input = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(prefix, clrNamespace),
                P.EmptyElement(type, RootNs),
            };

            var expectedInstructions = new List<XamlInstruction>
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
            var input = protoPack.ExpandedStringProperty;

            var expectedInstructions = source.ExpandedStringProperty;

            var actualNodes = sut.Parse(input);
            var xamlNodes = actualNodes.ToList();

            CollectionAssert.AreEqual(expectedInstructions.ToList(), xamlNodes);
        }

        [TestMethod]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");
            var input = protoPack.GetString(sysNs);

            var expectedInstructions = source.GetString(sysNs);

            var actualNodes = sut.Parse(input);
            var xamlNodes = actualNodes.ToList();

            CollectionAssert.AreEqual(expectedInstructions.ToList(), xamlNodes);
        }
    }
}
