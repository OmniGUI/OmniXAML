namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Builder;
    using Classes;
    using Classes.Another;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.ProtoParser.SuperProtoParser;
    using OmniXaml.Parsers.XamlNodes;
    using Xaml.Tests.Resources;

    [TestClass]    
    public class RealXmlParsing : GivenAWiringContext
    {
        private readonly XamlNodeBuilder nodeBuilder;

        public RealXmlParsing()
        {
            nodeBuilder = new XamlNodeBuilder(WiringContext.TypeContext);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void EmptyString()
        {
            ExtractNodesFromPullParser(string.Empty);
        }

        [TestMethod]
        public void SingleInstance()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof (DummyClass)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.SingleInstance);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void RootNamespace()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.RootNamespace);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.Value("Property!"),
                nodeBuilder.EndMember(),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.StringProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        [Description("Se queda tronchado")]
        public void InstanceWithChild()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                //nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Child),
                nodeBuilder.StartObject(typeof(ChildClass)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.InstanceWithChild);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void DifferentNamespaces()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration("root", string.Empty),
                nodeBuilder.NamespacePrefixDeclaration("another", "x"),
                nodeBuilder.StartObject(typeof(DummyClass)),
                //nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.ChildFromAnotherNamespace),
                nodeBuilder.StartObject(typeof(Foreigner)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.DifferentNamespaces);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void DifferentNamespacesAndMoreThanOneProperty()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration("root", string.Empty),
                nodeBuilder.NamespacePrefixDeclaration("another", "x"),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.Value("One"),
                nodeBuilder.EndMember(),
                nodeBuilder.StartMember<DummyClass>(d => d.AnotherProperty),
                nodeBuilder.Value("Two"),
                nodeBuilder.EndMember(),
                //nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.ChildFromAnotherNamespace),
                nodeBuilder.StartObject(typeof(Foreigner)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.DifferentNamespacesAndMoreThanOneProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ClassWithInnerCollection()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                //nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof(Item)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ClassWithInnerCollection);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollectionWithMoreThanOneItem()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                //nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof(Item)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.StartObject(typeof(Item)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.StartObject(typeof(Item)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollectionWithMoreThanOneItem);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollapsedTagWithProperty()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.Value("Property!"),
                nodeBuilder.EndMember(),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollapsedTagWithProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void CollectionWithClosedItemAndProperty()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                //nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.StartMember<Item>(d => d.Title),
                nodeBuilder.Value("SomeText"),
                nodeBuilder.EndMember(),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollectionWithClosedItemAndProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void SimpleExtension()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.StartObject(typeof(DummyExtension)),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.SimpleExtension);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]        
        public void SimpleExtensionWithOneAssignment()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.StartObject(typeof(DummyExtension)),
                nodeBuilder.StartMember<DummyExtension>(d => d.Property),
                nodeBuilder.Value("SomeProperty"),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.SimpleExtensionWithOneAssignment);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionOneElement()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                //nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof(Item)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForCollectionOneElement);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionMoreThanOneElement()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(DummyClass)),
                //nodeBuilder.None(),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof(Item)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.StartObject(typeof(Item)),
                //nodeBuilder.None(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForCollectionMoreThanOneElement);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForSingleProperty()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.StartObject(typeof(ChildClass)),
                nodeBuilder.StartMember<ChildClass>(d => d.Content),
                nodeBuilder.StartObject(typeof(Item)),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForSingleMember);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }

        private ICollection<XamlNode> ExtractNodesFromPullParser(string xml)
        {
            var pullParser = new XamlNodesPullParser(WiringContext);
            return pullParser.Parse(new SuperProtoParser(WiringContext).Parse(xml)).ToList();
        }

        [TestMethod]
        public void KeyDirective()
        {
            var expectedNodes = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration(RootNs),
                nodeBuilder.NamespacePrefixDeclaration("http://schemas.microsoft.com/winfx/2006/xaml", "x"),
                nodeBuilder.StartObject(typeof (DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.Resources),

                nodeBuilder.GetObject(),
                nodeBuilder.Items(),

                nodeBuilder.StartObject(typeof(ChildClass)),
                nodeBuilder.StartDirective("Key"),
                nodeBuilder.Value("One"),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),

                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.KeyDirective);

            XamlNodesAssert.AreEssentiallyTheSame(expectedNodes, actualNodes);
        }
    }
}