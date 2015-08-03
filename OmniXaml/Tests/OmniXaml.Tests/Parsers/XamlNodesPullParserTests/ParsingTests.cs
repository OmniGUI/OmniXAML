namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Builder;
    using Classes;
    using Classes.WpfLikeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.XamlNodes;

    [TestClass]
    public class ParsingTests : GivenAWiringContext
    {
        private readonly ProtoNodeBuilder p;
        private readonly XamlNodeBuilder x;
        private readonly XamlNodesPullParser sut;
        private readonly SampleData sampleData;

        public ParsingTests()
        {
            p = new ProtoNodeBuilder(WiringContext.TypeContext);
            x = new XamlNodeBuilder(WiringContext.TypeContext);
            sut = new XamlNodesPullParser(WiringContext);
            sampleData = new SampleData(p, x);
        }

        [TestMethod]
        public void NamespaceDeclarationOnly()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
            };


            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void SingleInstanceCollapsed()
        {
            var input = new List<ProtoXamlNode>
            {
                p.EmptyElement(typeof(DummyClass), rootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.StartObject<DummyClass>(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void SingleOpenAndClose()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NonEmptyElement(typeof(DummyClass), rootNs),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.StartObject<DummyClass>(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void EmptyElementWithStringProperty()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.EmptyElement(typeof (DummyClass), rootNs),
                p.Attribute<DummyClass>(d => d.SampleProperty, "Property!", rootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject<DummyClass>(),
                x.StartMember<DummyClass>(c => c.SampleProperty),
                x.Value("Property!"),
                x.EndMember(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void EmptyElementWithTwoStringProperties()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.EmptyElement(typeof (DummyClass), rootNs),
                p.Attribute<DummyClass>(d => d.SampleProperty, "Property!", rootNs),
                p.Attribute<DummyClass>(d => d.AnotherProperty, "Come on!", rootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject<DummyClass>(),
                x.StartMember<DummyClass>(c => c.SampleProperty),
                x.Value("Property!"),
                x.EndMember(),
                x.StartMember<DummyClass>(c => c.AnotherProperty),
                x.Value("Come on!"),
                x.EndMember(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void SingleCollapsedWithNs()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.EmptyElement(typeof(DummyClass), rootNs),
                p.None()
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject<DummyClass>(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NamespacePrefixDeclaration(anotherNs),
                p.EmptyElement(typeof(DummyClass), rootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.NamespacePrefixDeclaration(anotherNs),
                x.StartObject<DummyClass>(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void ElementWithNestedChild()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NonEmptyElement(typeof (DummyClass), rootNs),
                    p.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                        p.EmptyElement(typeof (ChildClass), rootNs),
                        p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.StartObject<DummyClass>(),
                    x.StartMember<DummyClass>(c => c.Child),
                        x.StartObject<ChildClass>(),
                        x.EndObject(),
                    x.EndMember(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void ComplexNesting()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (DummyClass), rootNs),
                    p.Attribute<DummyClass>(@class => @class.SampleProperty, "Sample", rootNs),
                    p.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                        p.NonEmptyElement(typeof (ChildClass), rootNs),
                            p.NonEmptyPropertyElement<ChildClass>(d => d.Content, rootNs),
                                p.EmptyElement(typeof (Item), rootNs),
                                    p.Attribute<Item>(@class => @class.Text, "Value!", rootNs),
                                p.Text(),
                            p.EndTag(),
                        p.EndTag(),
                        p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject<DummyClass>(),
                    x.StartMember<DummyClass>(c => c.SampleProperty),
                        x.Value("Sample"),
                    x.EndMember(),
                    x.StartMember<DummyClass>(c => c.Child),
                        x.StartObject<ChildClass>(),
                            x.StartMember<ChildClass>(c => c.Content),
                                x.StartObject<Item>(),
                                    x.StartMember<Item>(d => d.Text),
                                        x.Value("Value!"),
                                    x.EndMember(),
                                x.EndObject(),
                            x.EndMember(),
                        x.EndObject(),
                    x.EndMember(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ChildCollection()
        {

            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (DummyClass), rootNs),
                    p.NonEmptyPropertyElement<DummyClass>(d => d.Items, rootNs),
                        p.EmptyElement<Item>(rootNs),
                            p.Text(),
                        p.EmptyElement<Item>(rootNs),
                            p.Text(),
                        p.EmptyElement<Item>(rootNs),
                            p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof(DummyClass)),
                    x.StartMember<DummyClass>(d => d.Items),
                        x.GetObject(),
                            x.Items(),
                                x.StartObject(typeof(Item)),
                                x.EndObject(),
                                x.StartObject(typeof(Item)),
                                x.EndObject(),
                                x.StartObject(typeof(Item)),
                                x.EndObject(),
                            x.EndMember(),
                        x.EndObject(),
                    x.EndMember(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void NestedChildWithContentProperty()
        {

            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (ChildClass), rootNs),
                    p.EmptyElement(typeof (Item), rootNs),
                    p.Text(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject<ChildClass>(),
                    x.StartMember<ChildClass>(c => c.Content),
                        x.StartObject<Item>(),
                        x.EndObject(),
                    x.EndMember(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void NestedCollectionWithContentProperty()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (DummyClass), rootNs),
                    p.EmptyElement<Item>(rootNs),
                        p.Text(),
                    p.EmptyElement<Item>(rootNs),
                        p.Text(),
                    p.EmptyElement<Item>(rootNs),
                        p.Text(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof(DummyClass)),
                    x.StartMember<DummyClass>(d => d.Items),
                        x.GetObject(),
                            x.Items(),
                                x.StartObject(typeof(Item)),
                                x.EndObject(),
                                x.StartObject(typeof(Item)),
                                x.EndObject(),
                                x.StartObject(typeof(Item)),
                                x.EndObject(),
                            x.EndMember(),
                        x.EndObject(),
                    x.EndMember(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollectionsContentPropertyNesting()
        {
            var input = sampleData.CreateInputForCollectionsContentPropertyNesting(rootNs);
            var actualNodes = sut.Parse(input).ToList();
            var expectedNodes = sampleData.CreateExpectedNodesCollectionsContentPropertyNesting(rootNs);

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void TwoNestedProperties()
        {
            var input = sampleData.CreateInputForTwoNestedProperties(rootNs);
            var actualNodes = sut.Parse(input).ToList();
            var expectedNodes = sampleData.CreateExpectedNodesForTwoNestedProperties(rootNs);

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesUsingContentProperty()
        {

            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (DummyClass), rootNs),

                    p.EmptyElement(typeof (Item), rootNs),
                    p.Attribute<Item>(d => d.Title, "Main1", rootNs),
                    p.Text(),

                    p.EmptyElement(typeof (Item), rootNs),
                    p.Attribute<Item>(d => d.Title, "Main2", rootNs),
                    p.Text(),

                    p.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                        p.NonEmptyElement(typeof(ChildClass), rootNs),
                        p.EndTag(),
                        p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof(DummyClass)),
                    x.StartMember<DummyClass>(d => d.Items),
                        x.GetObject(),
                            x.Items(),
                                x.StartObject(typeof(Item)),
                                    x.StartMember<Item>(i => i.Title),
                                        x.Value("Main1"),
                                    x.EndMember(),
                                x.EndObject(),
                                x.StartObject(typeof(Item)),
                                    x.StartMember<Item>(i => i.Title),
                                        x.Value("Main2"),
                                    x.EndMember(),
                                x.EndObject(),
                            x.EndMember(),
                        x.EndObject(),
                    x.EndMember(),
                    x.StartMember<DummyClass>(d => d.Child),
                        x.StartObject(typeof(ChildClass)),
                        x.EndObject(),
                    x.EndMember(),
                x.EndObject(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem()
        {

            var input = sampleData.CreateInputForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(rootNs);

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = sampleData.CreateExpectedNodesForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(rootNs);

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void MixedPropertiesWithContentPropertyAfter()
        {
            var input = (IEnumerable<ProtoXamlNode>)new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (Grid), rootNs),
                    p.NonEmptyPropertyElement<Grid>(g => g.RowDefinitions, rootNs),
                        p.EmptyElement(typeof (RowDefinition), rootNs),
                    p.EndTag(),
                    p.EmptyElement<TextBlock>(rootNs),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof(Grid)),
                    x.StartMember<Grid>(d => d.RowDefinitions),
                        x.GetObject(),
                            x.Items(),
                                x.StartObject(typeof(RowDefinition)),
                                x.EndObject(),
                            x.EndMember(),
                        x.EndObject(),
                    x.EndMember(),
                     x.StartMember<Grid>(d => d.Children),
                        x.GetObject(),
                            x.Items(),
                                x.StartObject(typeof(TextBlock)),
                                x.EndObject(),
                            x.EndMember(),
                        x.EndObject(),
                    x.EndMember(),
                x.EndObject(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollectionWithMixedEmptyAndNotEmptyNestedElements()
        {
            var input = (IEnumerable<ProtoXamlNode>)new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (Grid), rootNs),
                    p.NonEmptyPropertyElement<Grid>(g => g.Children, rootNs),
                        p.NonEmptyElement(typeof (TextBlock), rootNs),
                        p.EndTag(),
                        p.Text(),
                        p.EmptyElement(typeof (TextBlock), rootNs),
                        p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof(Grid)),
                    x.StartMember<Grid>(d => d.Children),
                        x.GetObject(),
                            x.Items(),
                                x.StartObject(typeof(TextBlock)),
                                x.EndObject(),
                                 x.StartObject(typeof(TextBlock)),
                                x.EndObject(),
                            x.EndMember(),
                        x.EndObject(),
                    x.EndMember(),
                x.EndObject(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void MixedPropertiesWithContentPropertyBefore()
        {
            var input = (IEnumerable<ProtoXamlNode>)new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (Grid), rootNs),
                    p.EmptyElement<TextBlock>(rootNs),
                    p.NonEmptyPropertyElement<Grid>(g => g.RowDefinitions, rootNs),
                        p.EmptyElement(typeof (RowDefinition), rootNs),
                    p.EndTag(),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject(typeof(Grid)),
                    x.StartMember<Grid>(d => d.Children),
                        x.GetObject(),
                            x.Items(),
                                x.StartObject(typeof(TextBlock)),
                                x.EndObject(),
                            x.EndMember(),
                        x.EndObject(),
                    x.EndMember(),
                    x.StartMember<Grid>(d => d.RowDefinitions),
                        x.GetObject(),
                            x.Items(),
                                x.StartObject(typeof(RowDefinition)),
                                x.EndObject(),
                            x.EndMember(),
                        x.EndObject(),
                    x.EndMember(),
                x.EndObject(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ImplicitContentPropertyWithImplicityCollection()
        {
            var input = sampleData.CreateInputForImplicitContentPropertyWithImplicityCollection(rootNs);

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = sampleData.CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection(rootNs);

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ClrNamespace()
        {
            var type = typeof(DummyClass);
            string clrNamespace = $"clr-namespace:{type.Namespace};Assembly={type.GetTypeInfo().Assembly.GetName().Name}";
            var prefix = "prefix";
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(prefix, clrNamespace),
                p.EmptyElement(type, rootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(clrNamespace, prefix),
                x.StartObject<DummyClass>(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void ExpandedStringProperty()
        {
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(rootNs),
                p.NonEmptyElement(typeof (DummyClass), rootNs),
                p.NonEmptyPropertyElement<DummyClass>(d => d.SampleProperty, rootNs),
                p.Text("Property!"),
                p.EndTag(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs),
                x.StartObject<DummyClass>(),
                x.StartMember<DummyClass>(c => c.SampleProperty),
                x.Value("Property!"),
                x.EndMember(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);
            var xamlNodes = actualNodes.ToList();

            CollectionAssert.AreEqual(expectedNodes, xamlNodes);
        }
    }
}
