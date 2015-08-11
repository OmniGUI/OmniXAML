namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Classes;
    using Classes.Another;
    using Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlNodes;
    using Xaml.Tests.Resources;

    [TestClass]    
    public class RealXmlParsing : GivenAWiringContextWithNodeBuilders
    {
        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void EmptyString()
        {
            ExtractNodesFromPullParser(string.Empty);
        }

        [TestMethod]
        public void SingleInstance()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof (DummyClass)),
                //X.None(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.SingleInstance);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void RootNamespace()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                //X.None(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.RootNamespace);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                X.StartMember<DummyClass>(d => d.SampleProperty),
                X.Value("Property!"),
                X.EndMember(),
                //X.None(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.StringProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        [Description("Se queda tronchado")]
        public void InstanceWithChild()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                //X.None(),
                X.StartMember<DummyClass>(d => d.Child),
                X.StartObject(typeof(ChildClass)),
                //X.None(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.InstanceWithChild);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void DifferentNamespaces()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration("root", string.Empty),
                X.NamespacePrefixDeclaration("another", "x"),
                X.StartObject(typeof(DummyClass)),
                //X.None(),
                X.StartMember<DummyClass>(d => d.ChildFromAnotherNamespace),
                X.StartObject(typeof(Foreigner)),
                //X.None(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.DifferentNamespaces);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void DifferentNamespacesAndMoreThanOneProperty()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration("root", string.Empty),
                X.NamespacePrefixDeclaration("another", "x"),
                X.StartObject(typeof(DummyClass)),
                X.StartMember<DummyClass>(d => d.SampleProperty),
                X.Value("One"),
                X.EndMember(),
                X.StartMember<DummyClass>(d => d.AnotherProperty),
                X.Value("Two"),
                X.EndMember(),
                //X.None(),
                X.StartMember<DummyClass>(d => d.ChildFromAnotherNamespace),
                X.StartObject(typeof(Foreigner)),
                //X.None(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.DifferentNamespacesAndMoreThanOneProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ClassWithInnerCollection()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                //X.None(),
                X.StartMember<DummyClass>(d => d.Items),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof(Item)),
                //X.None(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ClassWithInnerCollection);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void CollectionWithMoreThanOneItem()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                //X.None(),
                X.StartMember<DummyClass>(d => d.Items),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof(Item)),
                //X.None(),
                X.EndObject(),
                X.StartObject(typeof(Item)),
                //X.None(),
                X.EndObject(),
                X.StartObject(typeof(Item)),
                //X.None(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollectionWithMoreThanOneItem);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void CollapsedTagWithProperty()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                X.StartMember<DummyClass>(d => d.SampleProperty),
                X.Value("Property!"),
                X.EndMember(),
                //X.None(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollapsedTagWithProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void CollectionWithClosedItemAndProperty()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                //X.None(),
                X.StartMember<DummyClass>(d => d.Items),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof(Item)),
                X.StartMember<Item>(d => d.Title),
                X.Value("SomeText"),
                X.EndMember(),
                //X.None(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollectionWithClosedItemAndProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void SimpleExtension()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                X.StartMember<DummyClass>(d => d.SampleProperty),
                X.StartObject(typeof(DummyExtension)),
                X.EndObject(),
                X.EndMember(),
                //X.None(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.SimpleExtension);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]        
        public void SimpleExtensionWithOneAssignment()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                X.StartMember<DummyClass>(d => d.SampleProperty),
                X.StartObject(typeof(DummyExtension)),
                X.StartMember<DummyExtension>(d => d.Property),
                X.Value("SomeProperty"),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                //X.None(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.SimpleExtensionWithOneAssignment);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionOneElement()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                //X.None(),
                X.StartMember<DummyClass>(d => d.Items),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof(Item)),
                //X.None(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForCollectionOneElement);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionMoreThanOneElement()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(DummyClass)),
                //X.None(),
                X.StartMember<DummyClass>(d => d.Items),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof(Item)),
                //X.None(),
                X.EndObject(),
                X.StartObject(typeof(Item)),
                //X.None(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForCollectionMoreThanOneElement);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForSingleProperty()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof(ChildClass)),
                X.StartMember<ChildClass>(d => d.Content),
                X.StartObject(typeof(Item)),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForSingleMember);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        private ICollection<XamlInstruction> ExtractNodesFromPullParser(string xml)
        {
            var pullParser = new XamlInstructionParser(WiringContext);
            return pullParser.Parse(new XamlProtoInstructionParser(WiringContext).Parse<IEnumerable<ProtoXamlInstruction>>(xml)).ToList();
        }

        [TestMethod]
        public void KeyDirective()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.NamespacePrefixDeclaration("http://schemas.microsoft.com/winfx/2006/xaml", "x"),
                X.StartObject(typeof (DummyClass)),
                X.StartMember<DummyClass>(d => d.Resources),

                X.GetObject(),
                X.Items(),

                X.StartObject(typeof(ChildClass)),
                X.StartDirective("Key"),
                X.Value("One"),
                X.EndMember(),
                X.EndObject(),

                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.KeyDirective);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }
    }
}