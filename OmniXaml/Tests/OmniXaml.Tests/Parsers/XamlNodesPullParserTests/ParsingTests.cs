namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Builder;
    using Classes;
    using Classes.WpfLikeModel;
    using Glass;
    using Glass.Tests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.XamlNodes;
    using Typing;

    [TestClass]
    public class ParsingTests : GivenAWiringContextWithNodeBuilders
    {
        private readonly IXamlNodesPullParser sut;
        private readonly SampleData sampleData;

        public ParsingTests()
        {          
            sut = new XamlNodesPullParser(WiringContext);
            sampleData = new SampleData(P, X);
        }

        [TestMethod]
        public void NamespaceDeclarationOnly()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
            };


            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void SingleInstanceCollapsed()
        {
            var input = new List<ProtoXamlNode>
            {
                P.EmptyElement(typeof(DummyClass), RootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.StartObject<DummyClass>(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void SingleOpenAndClose()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NonEmptyElement(typeof(DummyClass), RootNs),
                P.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.StartObject<DummyClass>(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void EmptyElementWithStringProperty()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement(typeof (DummyClass), RootNs),
                P.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<DummyClass>(),
                X.StartMember<DummyClass>(c => c.SampleProperty),
                X.Value("Property!"),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void EmptyElementWithTwoStringProperties()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement(typeof (DummyClass), RootNs),
                P.Attribute<DummyClass>(d => d.SampleProperty, "Property!", RootNs),
                P.Attribute<DummyClass>(d => d.AnotherProperty, "Come on!", RootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<DummyClass>(),
                X.StartMember<DummyClass>(c => c.SampleProperty),
                X.Value("Property!"),
                X.EndMember(),
                X.StartMember<DummyClass>(c => c.AnotherProperty),
                X.Value("Come on!"),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void SingleCollapsedWithNs()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.EmptyElement(typeof(DummyClass), RootNs),
                P.None()
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<DummyClass>(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void ElementWith2NsDeclarations()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NamespacePrefixDeclaration(AnotherNs),
                P.EmptyElement(typeof(DummyClass), RootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.NamespacePrefixDeclaration(AnotherNs),
                X.StartObject<DummyClass>(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void ElementWithNestedChild()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                        P.EmptyElement(typeof (ChildClass), RootNs),
                        P.Text(),
                    P.EndTag(),
                P.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(c => c.Child),
                        X.StartObject<ChildClass>(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void ComplexNesting()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.Attribute<DummyClass>(@class => @class.SampleProperty, "Sample", RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                        P.NonEmptyElement(typeof (ChildClass), RootNs),
                            P.NonEmptyPropertyElement<ChildClass>(d => d.Content, RootNs),
                                P.EmptyElement(typeof (Item), RootNs),
                                    P.Attribute<Item>(@class => @class.Text, "Value!", RootNs),
                                P.Text(),
                            P.EndTag(),
                        P.EndTag(),
                        P.Text(),
                    P.EndTag(),
                P.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(c => c.SampleProperty),
                        X.Value("Sample"),
                    X.EndMember(),
                    X.StartMember<DummyClass>(c => c.Child),
                        X.StartObject<ChildClass>(),
                            X.StartMember<ChildClass>(c => c.Content),
                                X.StartObject<Item>(),
                                    X.StartMember<Item>(d => d.Text),
                                        X.Value("Value!"),
                                    X.EndMember(),
                                X.EndObject(),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ChildCollection()
        {

            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.NonEmptyPropertyElement<DummyClass>(d => d.Items, RootNs),
                        P.EmptyElement<Item>(RootNs),
                            P.Text(),
                        P.EmptyElement<Item>(RootNs),
                            P.Text(),
                        P.EmptyElement<Item>(RootNs),
                            P.Text(),
                    P.EndTag(),
                P.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                    X.StartMember<DummyClass>(d => d.Items),
                        X.GetObject(),
                            X.Items(),
                                X.StartObject(typeof(Item)),
                                X.EndObject(),
                                X.StartObject(typeof(Item)),
                                X.EndObject(),
                                X.StartObject(typeof(Item)),
                                X.EndObject(),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void NestedChildWithContentProperty()
        {

            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (ChildClass), RootNs),
                    P.EmptyElement(typeof (Item), RootNs),
                    P.Text(),
                P.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<ChildClass>(),
                    X.StartMember<ChildClass>(c => c.Content),
                        X.StartObject<Item>(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input).ToList();

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void NestedCollectionWithContentProperty()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (DummyClass), RootNs),
                    P.EmptyElement<Item>(RootNs),
                        P.Text(),
                    P.EmptyElement<Item>(RootNs),
                        P.Text(),
                    P.EmptyElement<Item>(RootNs),
                        P.Text(),
                P.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                    X.StartMember<DummyClass>(d => d.Items),
                        X.GetObject(),
                            X.Items(),
                                X.StartObject(typeof(Item)),
                                X.EndObject(),
                                X.StartObject(typeof(Item)),
                                X.EndObject(),
                                X.StartObject(typeof(Item)),
                                X.EndObject(),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject(),
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
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (DummyClass), RootNs),

                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Main1", RootNs),
                    P.Text(),

                    P.EmptyElement(typeof (Item), RootNs),
                    P.Attribute<Item>(d => d.Title, "Main2", RootNs),
                    P.Text(),

                    P.NonEmptyPropertyElement<DummyClass>(d => d.Child, RootNs),
                        P.NonEmptyElement(typeof(ChildClass), RootNs),
                        P.EndTag(),
                        P.Text(),
                    P.EndTag(),
                P.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                    X.StartMember<DummyClass>(d => d.Items),
                        X.GetObject(),
                            X.Items(),
                                X.StartObject(typeof(Item)),
                                    X.StartMember<Item>(i => i.Title),
                                        X.Value("Main1"),
                                    X.EndMember(),
                                X.EndObject(),
                                X.StartObject(typeof(Item)),
                                    X.StartMember<Item>(i => i.Title),
                                        X.Value("Main2"),
                                    X.EndMember(),
                                X.EndObject(),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                    X.StartMember<DummyClass>(d => d.Child),
                        X.StartObject(typeof(ChildClass)),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject(),
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
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (Grid), RootNs),
                    P.NonEmptyPropertyElement<Grid>(g => g.RowDefinitions, RootNs),
                        P.EmptyElement(typeof (RowDefinition), RootNs),
                    P.EndTag(),
                    P.EmptyElement<TextBlock>(RootNs),
                P.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(Grid)),
                    X.StartMember<Grid>(d => d.RowDefinitions),
                        X.GetObject(),
                            X.Items(),
                                X.StartObject(typeof(RowDefinition)),
                                X.EndObject(),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                     X.StartMember<Grid>(d => d.Children),
                        X.GetObject(),
                            X.Items(),
                                X.StartObject(typeof(TextBlock)),
                                X.EndObject(),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollectionWithMixedEmptyAndNotEmptyNestedElements()
        {
            var input = (IEnumerable<ProtoXamlNode>)new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (Grid), RootNs),
                    P.NonEmptyPropertyElement<Grid>(g => g.Children, RootNs),
                        P.NonEmptyElement(typeof (TextBlock), RootNs),
                        P.EndTag(),
                        P.Text(),
                        P.EmptyElement(typeof (TextBlock), RootNs),
                        P.Text(),
                    P.EndTag(),
                P.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(Grid)),
                    X.StartMember<Grid>(d => d.Children),
                        X.GetObject(),
                            X.Items(),
                                X.StartObject(typeof(TextBlock)),
                                X.EndObject(),
                                 X.StartObject(typeof(TextBlock)),
                                X.EndObject(),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void MixedPropertiesWithContentPropertyBefore()
        {
            var input = (IEnumerable<ProtoXamlNode>)new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (Grid), RootNs),
                    P.EmptyElement<TextBlock>(RootNs),
                    P.NonEmptyPropertyElement<Grid>(g => g.RowDefinitions, RootNs),
                        P.EmptyElement(typeof (RowDefinition), RootNs),
                    P.EndTag(),
                P.EndTag(),
            };

            var actualNodes = sut.Parse(input).ToList();

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(Grid)),
                    X.StartMember<Grid>(d => d.Children),
                        X.GetObject(),
                            X.Items(),
                                X.StartObject(typeof(TextBlock)),
                                X.EndObject(),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                    X.StartMember<Grid>(d => d.RowDefinitions),
                        X.GetObject(),
                            X.Items(),
                                X.StartObject(typeof(RowDefinition)),
                                X.EndObject(),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject(),
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
                P.NamespacePrefixDeclaration(prefix, clrNamespace),
                P.EmptyElement(type, RootNs),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(clrNamespace, prefix),
                X.StartObject<DummyClass>(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input);

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void ExpandedStringProperty()
        {
            var input = new List<ProtoXamlNode>
            {
                P.NamespacePrefixDeclaration(RootNs),
                P.NonEmptyElement(typeof (DummyClass), RootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.SampleProperty, RootNs),
                P.Text("Property!"),
                P.EndTag(),
                P.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject<DummyClass>(),
                X.StartMember<DummyClass>(c => c.SampleProperty),
                X.Value("Property!"),
                X.EndMember(),
                X.EndObject(),
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
                P.NamespacePrefixDeclaration(sysNs),
                P.NonEmptyElement(typeof (string), sysNs),
                P.Text("Text"),
                P.EndTag(),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.NamespacePrefixDeclaration(sysNs),
                X.StartObject<string>(),
                X.StartDirective("_Initialization"),
                X.Value("Text"),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = sut.Parse(input);
            var xamlNodes = actualNodes.ToList();

            CollectionAssert.AreEqual(expectedNodes, xamlNodes);
        }

        [TestMethod]
        public void SortMembers()
        {
            var input = new List<XamlNode>
            {
                X.StartMember<Setter>(c => c.Value),
                X.Value("Value"),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Property),
                X.Value("Property"),
                X.EndMember(),
            };

            var expectedNodes = new List<XamlNode>
            {
                X.StartMember<Setter>(c => c.Property),
                X.Value("Property"),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Value),
                X.Value("Value"),
                X.EndMember(),

            };

            var actualNodes = ParserMierda(input).ToList();
            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        private IEnumerable<XamlNode> ParserMierda(List<XamlNode> xamlNodes)
        {
            var blocks = CollectBlocks(xamlNodes);
            LinkBlocks(blocks);
            return SortQueues(blocks);
        }

        private Collection<MemberNodesBlock> CollectBlocks(List<XamlNode> expectedNodes)
        {
            var enumerator = expectedNodes.GetEnumerator();
            var queues = new Collection<MemberNodesBlock>();
            var isRecording = false;
            MemberNodesBlock currentBlock = null;

            while (enumerator.MoveNext())
            {
                var xamlNode = enumerator.Current;

                if (IsStartOfMember(xamlNode))
                {
                    isRecording = true;
                    currentBlock = new MemberNodesBlock(xamlNode);
                    queues.Add(currentBlock);
                }
                else if (IsEndOfMember(xamlNode))
                {
                    isRecording = false;
                }

                if (isRecording)
                {
                    currentBlock.AddNode(xamlNode);
                }
            }

            return queues;
        }

        private void LinkBlocks(Collection<MemberNodesBlock> blocks)
        {
            foreach (var block in blocks)
            {
                block.Link(blocks);
            }
        }

        private static IEnumerable<XamlNode> SortQueues(IEnumerable<MemberNodesBlock> queues)
        {
            var memberNodesBlock = queues.First();
            var sortedBlocks = memberNodesBlock.Sort();

            return sortedBlocks.SelectMany(sortedBlock => sortedBlock.Nodes);
        }

        private bool IsEndOfMember(XamlNode xamlNode)
        {
            return xamlNode.NodeType == XamlNodeType.EndMember && xamlNode.Member is MutableXamlMember;
        }

        private static bool IsStartOfMember(XamlNode xamlNode)
        {
            return xamlNode.NodeType == XamlNodeType.StartMember && xamlNode.Member is MutableXamlMember;
        }
    }
}
