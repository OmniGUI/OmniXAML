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
    using OmniXaml.Parsers.XamlNodes;
    using Xaml.Tests.Resources;

    [TestClass]    
    public class RealXmlParsing : GivenAWiringContext
    {
        private readonly XamlInstructionBuilder instructionBuilder;

        public RealXmlParsing()
        {
            instructionBuilder = new XamlInstructionBuilder(WiringContext.TypeContext);
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
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof (DummyClass)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.SingleInstance);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void RootNamespace()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.RootNamespace);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                instructionBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                instructionBuilder.Value("Property!"),
                instructionBuilder.EndMember(),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
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
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                //instructionBuilder.None(),
                instructionBuilder.StartMember<DummyClass>(d => d.Child),
                instructionBuilder.StartObject(typeof(ChildClass)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.InstanceWithChild);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void DifferentNamespaces()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration("root", string.Empty),
                instructionBuilder.NamespacePrefixDeclaration("another", "x"),
                instructionBuilder.StartObject(typeof(DummyClass)),
                //instructionBuilder.None(),
                instructionBuilder.StartMember<DummyClass>(d => d.ChildFromAnotherNamespace),
                instructionBuilder.StartObject(typeof(Foreigner)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.DifferentNamespaces);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void DifferentNamespacesAndMoreThanOneProperty()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration("root", string.Empty),
                instructionBuilder.NamespacePrefixDeclaration("another", "x"),
                instructionBuilder.StartObject(typeof(DummyClass)),
                instructionBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                instructionBuilder.Value("One"),
                instructionBuilder.EndMember(),
                instructionBuilder.StartMember<DummyClass>(d => d.AnotherProperty),
                instructionBuilder.Value("Two"),
                instructionBuilder.EndMember(),
                //instructionBuilder.None(),
                instructionBuilder.StartMember<DummyClass>(d => d.ChildFromAnotherNamespace),
                instructionBuilder.StartObject(typeof(Foreigner)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.DifferentNamespacesAndMoreThanOneProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ClassWithInnerCollection()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                //instructionBuilder.None(),
                instructionBuilder.StartMember<DummyClass>(d => d.Items),
                instructionBuilder.GetObject(),
                instructionBuilder.Items(),
                instructionBuilder.StartObject(typeof(Item)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ClassWithInnerCollection);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void CollectionWithMoreThanOneItem()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                //instructionBuilder.None(),
                instructionBuilder.StartMember<DummyClass>(d => d.Items),
                instructionBuilder.GetObject(),
                instructionBuilder.Items(),
                instructionBuilder.StartObject(typeof(Item)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.StartObject(typeof(Item)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.StartObject(typeof(Item)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollectionWithMoreThanOneItem);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void CollapsedTagWithProperty()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                instructionBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                instructionBuilder.Value("Property!"),
                instructionBuilder.EndMember(),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollapsedTagWithProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void CollectionWithClosedItemAndProperty()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                //instructionBuilder.None(),
                instructionBuilder.StartMember<DummyClass>(d => d.Items),
                instructionBuilder.GetObject(),
                instructionBuilder.Items(),
                instructionBuilder.StartObject(typeof(Item)),
                instructionBuilder.StartMember<Item>(d => d.Title),
                instructionBuilder.Value("SomeText"),
                instructionBuilder.EndMember(),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollectionWithClosedItemAndProperty);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void SimpleExtension()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                instructionBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                instructionBuilder.StartObject(typeof(DummyExtension)),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.SimpleExtension);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]        
        public void SimpleExtensionWithOneAssignment()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                instructionBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                instructionBuilder.StartObject(typeof(DummyExtension)),
                instructionBuilder.StartMember<DummyExtension>(d => d.Property),
                instructionBuilder.Value("SomeProperty"),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.SimpleExtensionWithOneAssignment);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionOneElement()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                //instructionBuilder.None(),
                instructionBuilder.StartMember<DummyClass>(d => d.Items),
                instructionBuilder.GetObject(),
                instructionBuilder.Items(),
                instructionBuilder.StartObject(typeof(Item)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForCollectionOneElement);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForCollectionMoreThanOneElement()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(DummyClass)),
                //instructionBuilder.None(),
                instructionBuilder.StartMember<DummyClass>(d => d.Items),
                instructionBuilder.GetObject(),
                instructionBuilder.Items(),
                instructionBuilder.StartObject(typeof(Item)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.StartObject(typeof(Item)),
                //instructionBuilder.None(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForCollectionMoreThanOneElement);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }

        [TestMethod]
        public void ContentPropertyForSingleProperty()
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.StartObject(typeof(ChildClass)),
                instructionBuilder.StartMember<ChildClass>(d => d.Content),
                instructionBuilder.StartObject(typeof(Item)),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
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
                instructionBuilder.NamespacePrefixDeclaration(RootNs),
                instructionBuilder.NamespacePrefixDeclaration("http://schemas.microsoft.com/winfx/2006/xaml", "x"),
                instructionBuilder.StartObject(typeof (DummyClass)),
                instructionBuilder.StartMember<DummyClass>(d => d.Resources),

                instructionBuilder.GetObject(),
                instructionBuilder.Items(),

                instructionBuilder.StartObject(typeof(ChildClass)),
                instructionBuilder.StartDirective("Key"),
                instructionBuilder.Value("One"),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),

                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
                instructionBuilder.EndMember(),
                instructionBuilder.EndObject(),
            };

            var actualNodes = ExtractNodesFromPullParser(Dummy.KeyDirective);

            XamlNodesAssert.AreEssentiallyTheSame(expectedInstructions, actualNodes);
        }
    }
}