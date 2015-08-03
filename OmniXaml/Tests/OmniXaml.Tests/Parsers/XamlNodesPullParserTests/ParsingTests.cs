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
                p.NamespacePrefixDeclaration(RootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
            };


            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void SingleInstanceCollapsed()
        {
            var input = new List<ProtoXamlNode>
            {
                p.EmptyElement(typeof(DummyClass), RootNs),
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
                p.NonEmptyElement(typeof(DummyClass), RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.EmptyElement(typeof (DummyClass), RootNs),
                p.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.EmptyElement(typeof (DummyClass), RootNs),
                p.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs),
                p.Attribute<DummyClass>(d => d.AnotherProperty, "Come on!", RootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.EmptyElement(typeof(DummyClass), RootNs),
                p.None()
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.NamespacePrefixDeclaration(AnotherNs),
                p.EmptyElement(typeof(DummyClass), RootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
                x.NamespacePrefixDeclaration(AnotherNs),
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
                p.NonEmptyElement(typeof (DummyClass), RootNs),
                    p.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                        p.EmptyElement(typeof (ChildClass), RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.NonEmptyElement(typeof (DummyClass), RootNs),
                    p.Attribute<DummyClass>(@class => @class.SampleProperty, "Sample", RootNs),
                    p.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                        p.NonEmptyElement(typeof (ChildClass), RootNs),
                            p.NonEmptyPropertyElement<ChildClass>(d => d.Content, RootNs),
                                p.EmptyElement(typeof (Item), RootNs),
                                    p.Attribute<Item>(@class => @class.Text, "Value!", RootNs),
                                p.Text(),
                            p.EndTag(),
                        p.EndTag(),
                        p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.NonEmptyElement(typeof (DummyClass), RootNs),
                    p.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                        p.EmptyElement<Item>(RootNs),
                            p.Text(),
                        p.EmptyElement<Item>(RootNs),
                            p.Text(),
                        p.EmptyElement<Item>(RootNs),
                            p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.NonEmptyElement(typeof (ChildClass), RootNs),
                    p.EmptyElement(typeof (Item), RootNs),
                    p.Text(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.NonEmptyElement(typeof (DummyClass), RootNs),
                    p.EmptyElement<Item>(RootNs),
                        p.Text(),
                    p.EmptyElement<Item>(RootNs),
                        p.Text(),
                    p.EmptyElement<Item>(RootNs),
                        p.Text(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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
            var input = sampleData.CreateInputForCollectionsContentPropertyNesting(RootNs);
            var actualNodes = sut.Parse(input).ToList();
            var expectedNodes = sampleData.CreateExpectedNodesCollectionsContentPropertyNesting(RootNs);

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void TwoNestedProperties()
        {
            var input = sampleData.CreateInputForTwoNestedProperties(RootNs);
            var actualNodes = sut.Parse(input).ToList();
            var expectedNodes = sampleData.CreateExpectedNodesForTwoNestedProperties(RootNs);

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void TwoNestedPropertiesUsingContentProperty()
        {

            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(RootNs),
                p.NonEmptyElement(typeof (DummyClass), RootNs),

                    p.EmptyElement(typeof (Item), RootNs),
                    p.Attribute<Item>(d => d.Title, "Main1", RootNs),
                    p.Text(),

                    p.EmptyElement(typeof (Item), RootNs),
                    p.Attribute<Item>(d => d.Title, "Main2", RootNs),
                    p.Text(),

                    p.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                        p.NonEmptyElement(typeof(ChildClass), RootNs),
                        p.EndTag(),
                        p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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

            var input = sampleData.CreateInputForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(RootNs);

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = sampleData.CreateExpectedNodesForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(RootNs);

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void MixedPropertiesWithContentPropertyAfter()
        {
            var input = (IEnumerable<ProtoXamlNode>)new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(RootNs),
                p.NonEmptyElement(typeof (Grid), RootNs),
                    p.NonEmptyPropertyElement<Grid>(g => g.RowDefinitions, RootNs),
                        p.EmptyElement(typeof (RowDefinition), RootNs),
                    p.EndTag(),
                    p.EmptyElement<TextBlock>(RootNs),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.NonEmptyElement(typeof (Grid), RootNs),
                    p.NonEmptyPropertyElement<Grid>(g => g.Children, RootNs),
                        p.NonEmptyElement(typeof (TextBlock), RootNs),
                        p.EndTag(),
                        p.Text(),
                        p.EmptyElement(typeof (TextBlock), RootNs),
                        p.Text(),
                    p.EndTag(),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.NonEmptyElement(typeof (Grid), RootNs),
                    p.EmptyElement<TextBlock>(RootNs),
                    p.NonEmptyPropertyElement<Grid>(g => g.RowDefinitions, RootNs),
                        p.EmptyElement(typeof (RowDefinition), RootNs),
                    p.EndTag(),
                p.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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
            var input = sampleData.CreateInputForImplicitContentPropertyWithImplicityCollection(RootNs);

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = sampleData.CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection(RootNs);

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
                p.EmptyElement(type, RootNs),
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
                p.NamespacePrefixDeclaration(RootNs),
                p.NonEmptyElement(typeof (DummyClass), RootNs),
                p.NonEmptyPropertyElement<DummyClass>(d => d.SampleProperty, RootNs),
                p.Text("Property!"),
                p.EndTag(),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(RootNs),
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

        [TestMethod]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");
            var input = new List<ProtoXamlNode>
            {
                p.NamespacePrefixDeclaration(sysNs),
                p.NonEmptyElement(typeof (string), sysNs),
                p.Text("Text"),
                p.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                x.NamespacePrefixDeclaration(sysNs),
                x.StartObject<string>(),
                x.StartDirective("_Initialization"),
                x.Value("Text"), 
                x.EndMember(),               
                x.EndObject(),
            };

            var actualNodes = sut.Parse(input);
            var xamlNodes = actualNodes.ToList();

            CollectionAssert.AreEqual(expectedNodes, xamlNodes);
        }
    }
}
