namespace OmniXaml.Tests.XamlPullParserTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using Classes;
    using Classes.Another;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xaml.Tests.Resources;

    [TestClass]
    public class ParsingTests : GivenAWiringContext
    {
        private readonly XamlNodeBuilder nodeBuilder;

        public ParsingTests()
        {
            nodeBuilder = new XamlNodeBuilder(WiringContext.TypeContext);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void EmptyString()
        {
            ExtractNodesFromPullParser(new StringReader(string.Empty));
        }

        [TestMethod]
        public void SingleInstance()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof (DummyClass)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.SingleInstance));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void RootNamespace()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.RootNamespace));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.Value("Property!"),
                nodeBuilder.EndMember(),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.InstanceWithStringPropertyAndNsDeclaration));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void InstanceWithChild()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Child),
                nodeBuilder.StartObject(typeof(ChildClass)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.InstanceWithChild));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void DifferentNamespaces()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", string.Empty),
                nodeBuilder.NamespaceDeclaration("another", "x"),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.ChildFromAnotherNamespace),
                nodeBuilder.StartObject(typeof(Foreigner)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.DifferentNamespaces));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void DifferentNamespacesAndMoreThanOneProperty()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", string.Empty),
                nodeBuilder.NamespaceDeclaration("another", "x"),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.Value("One"),
                nodeBuilder.EndMember(),
                nodeBuilder.StartMember<DummyClass>(d => d.AnotherProperty),
                nodeBuilder.Value("Two"),
                nodeBuilder.EndMember(),
                nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.ChildFromAnotherNamespace),
                nodeBuilder.StartObject(typeof(Foreigner)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.DifferentNamespacesAndMoreThanOneProperty));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ClassWithInnerCollection()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.ClassWithInnerCollection));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollectionWithMoreThanOneItem()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.CollectionWithMoreThanOneItem));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollapsedTagWithProperty()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.Value("Property!"),
                nodeBuilder.EndMember(),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.CollapsedTagWithProperty));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollectionWithClosedItemAndProperty()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.StartMember<Item>(d => d.Title),
                nodeBuilder.Value("SomeText"),
                nodeBuilder.EndMember(),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.CollectionWithClosedItemAndProperty));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void SimpleExtension()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.StartObject(typeof(DummyExtension)),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.SimpleExtension));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void SimpleExtensionWithOneAssignment()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.StartObject(typeof(DummyExtension)),
                nodeBuilder.StartMember<DummyExtension>(d => d.Property),
                nodeBuilder.Value("SomeProperty"),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.SimpleExtensionWithOneAssignment));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionOneElement()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.ContentPropertyForCollectionOneElement));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionMoreThanOneElement()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.ContentPropertyForCollectionMoreThanOneElement));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForSingleProperty()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespaceDeclaration("root", ""),
                nodeBuilder.StartObject(typeof(ChildClass)),
                nodeBuilder.None(),
                nodeBuilder.StartMember<ChildClass>(d => d.Content),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(new StringReader(Xaml.ContentPropertyForSingleMember));

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        private ICollection<XamlNode> ExtractNodesFromPullParser(TextReader stringReader)
        {
           throw new NotImplementedException();
        }
    }
}