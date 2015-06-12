namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
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
                p.NamespacePrefixDeclaration("", "root"),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""),
            };


            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void SingleInstanceCollapsed()
        {
            var input = new List<ProtoXamlNode>
            {
                p.EmptyElement(typeof(DummyClass), ""),
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
                p.NonEmptyElement(typeof(DummyClass),  string.Empty),
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
                p.NamespacePrefixDeclaration(string.Empty, "root"),
                p.EmptyElement(typeof (DummyClass), ""),
                p.Attribute<DummyClass>(d => d.SampleProperty, "Property!"),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", string.Empty),
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
                p.NamespacePrefixDeclaration(string.Empty, "root"),
                p.EmptyElement(typeof (DummyClass), ""),
                p.Attribute<DummyClass>(d => d.SampleProperty, "Property!"),
                p.Attribute<DummyClass>(d => d.AnotherProperty, "Come on!"),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", string.Empty),
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
            const string rootNs = "root";
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration("", rootNs),
                p.EmptyElement(typeof(DummyClass), ""),
                p.None()
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""),
                x.StartObject<DummyClass>(),
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            const string oneNamespace = "root";
            const string anotherNamespace = "another";

            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration("", oneNamespace),
                p.NamespacePrefixDeclaration("a", anotherNamespace),
                p.EmptyElement(typeof(DummyClass), ""),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(oneNamespace, ""),
                x.NamespacePrefixDeclaration(anotherNamespace, "a"),
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
                p.NonEmptyElement(typeof (DummyClass),  string.Empty),
                    p.NonEmptyPropertyElement<DummyClass>(d => d.Child, string.Empty),
                        p.EmptyElement(typeof (ChildClass), ""),
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
            const string rootNs = "root";
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (DummyClass),  string.Empty),
                    p.Attribute<DummyClass>(@class => @class.SampleProperty, "Sample"),
                    p.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                        p.NonEmptyElement(typeof (ChildClass),  string.Empty),
                            p.NonEmptyPropertyElement<ChildClass>(d => d.Content, rootNs),
                                p.EmptyElement(typeof (Item), ""),
                                    p.Attribute<Item>(@class => @class.Text, "Value!"),
                                p.Text(),
                            p.EndTag(),
                        p.EndTag(),
                        p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", string.Empty),
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
            const string rootNs = "root";
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (DummyClass),  string.Empty),
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
                x.NamespacePrefixDeclaration("root", ""),
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
            const string rootNs = "root";
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (ChildClass),  string.Empty),
                    p.EmptyElement(typeof (Item), ""),
                    p.Text(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(rootNs, string.Empty),
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
            const string rootNs = "root";
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (DummyClass), string.Empty),
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
                x.NamespacePrefixDeclaration("root", ""),
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
            var input = sampleData.CreateInputForCollectionsContentPropertyNesting();
            var actualNodes = sut.Parse(input).ToList();
            var expectedNodes = sampleData.CreateExpectedNodesCollectionsContentPropertyNesting();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void TwoNestedProperties()
        {
            var input = sampleData.CreateInputForTwoNestedProperties();
            var actualNodes = sut.Parse(input).ToList();
            var expectedNodes = sampleData.CreateExpectedNodesForTwoNestedProperties();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesUsingContentProperty()
        {
            const string rootNs = "root";
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (DummyClass), string.Empty),

                    p.EmptyElement(typeof (Item), ""),
                    p.Attribute<Item>(d => d.Title, "Main1"),
                    p.Text(),

                    p.EmptyElement(typeof (Item), ""),
                    p.Attribute<Item>(d => d.Title, "Main2"),
                    p.Text(),

                    p.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                        p.NonEmptyElement(typeof(ChildClass), string.Empty),
                        p.EndTag(),
                        p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""),
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
            const string rootNs = "root";
            var input = sampleData.CreateInputForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(rootNs);

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = sampleData.CreateExpectedNodesForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void MixedPropertiesWithContentPropertyAfter()
        {
            const string rootNs = "root";
            var input = (IEnumerable<ProtoXamlNode>)new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (Grid), string.Empty),
                    p.NonEmptyPropertyElement<Grid>(g => g.RowDefinitions, rootNs),
                        p.EmptyElement(typeof (RowDefinition), ""),
                    p.EndTag(),
                    p.EmptyElement<TextBlock>(rootNs),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""),
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
            const string rootNs = "root";
            var input = (IEnumerable<ProtoXamlNode>)new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (Grid), string.Empty),
                    p.NonEmptyPropertyElement<Grid>(g => g.Children, rootNs),
                        p.NonEmptyElement(typeof (TextBlock), string.Empty),
                        p.EndTag(),
                        p.Text(),
                        p.EmptyElement(typeof (TextBlock), ""),
                        p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""),
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
            const string rootNs = "root";
            var input = (IEnumerable<ProtoXamlNode>)new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(string.Empty, rootNs),
                p.NonEmptyElement(typeof (Grid), string.Empty),
                    p.EmptyElement<TextBlock>(rootNs),
                    p.NonEmptyPropertyElement<Grid>(g => g.RowDefinitions, rootNs),
                        p.EmptyElement(typeof (RowDefinition), ""),
                    p.EndTag(),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration("root", ""),
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
            var input = sampleData.CreateInputForImplicitContentPropertyWithImplicityCollection();

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = sampleData.CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }
    }
}
