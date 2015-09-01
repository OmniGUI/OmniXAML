namespace OmniXaml.Tests.Parsers.Integration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlInstructions;
    using Resources;
    using Xaml.Tests.Resources;

    [TestClass]
    public class FromXamlToInstructions : GivenAWiringContextWithNodeBuildersNetCore
    {
        private readonly XamlInstructionResources source;

        public FromXamlToInstructions()
        {
            source = new XamlInstructionResources(this);
        }

        [TestMethod]
        [ExpectedException(typeof (XmlException))]
        public void EmptyString()
        {
            ExtractNodesFromPullParser(string.Empty);
        }

        [TestMethod]
        public void SingleInstance()
        {
            var expectedInstructions = source.SingleInstance;

            var actualNodes = ExtractNodesFromPullParser(Dummy.SingleInstance);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }


        [TestMethod]
        public void RootNamespace()
        {
            var expectedInstructions = source.SingleInstance;

            var actualNodes = ExtractNodesFromPullParser(Dummy.RootNamespace);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }


        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var expectedInstructions = source.ObjectWithMember;

            var actualNodes = ExtractNodesFromPullParser(Dummy.StringProperty);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void InstanceWithChild()
        {
            var expectedInstructions = source.InstanceWithChild;

            var actualNodes = ExtractNodesFromPullParser(Dummy.InstanceWithChild);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void DifferentNamespaces()
        {
            var expectedInstructions = source.DifferentNamespaces;

            var actualNodes = ExtractNodesFromPullParser(Dummy.DifferentNamespaces);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }
        
        [TestMethod]
        public void DifferentNamespacesAndMoreThanOneProperty()
        {
            var expectedInstructions = source.DifferentNamespacesAndMoreThanOneProperty;

            var actualNodes = ExtractNodesFromPullParser(Dummy.DifferentNamespacesAndMoreThanOneProperty);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void ClassWithInnerCollection()
        {
            var expectedInstructions = source.CollectionWithOneItem;

            var actualNodes = ExtractNodesFromPullParser(Dummy.ClassWithInnerCollection);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void CollectionWithMoreThanOneItem()
        {
            var expectedInstructions = source.CollectionWithMoreThanOneItem;

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollectionWithMoreThanOneItem);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void CollapsedTagWithProperty()
        {
            var expectedInstructions = source.CollapsedTagWithProperty;

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollapsedTagWithProperty);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }


        [TestMethod]
        public void CollectionWithClosedItemAndProperty()
        {
            var expectedInstructions = source.CollectionWithOneItemAndAMember;

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollectionWithClosedItemAndProperty);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void SimpleExtension()
        {
            var expectedInstructions = source.SimpleExtension;

            var actualNodes = ExtractNodesFromPullParser(Dummy.SimpleExtension);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void SimpleExtensionWithOneAssignment()
        {
            var expectedInstructions = source.SimpleExtensionWithOneAssignment;

            var actualNodes = ExtractNodesFromPullParser(Dummy.SimpleExtensionWithOneAssignment);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void ContentPropertyForCollectionOneElement()
        {
            var expectedInstructions = source.ContentPropertyForCollectionOneElement;

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForCollectionOneElement);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void ContentPropertyForCollectionMoreThanOneElement()
        {
            var expectedInstructions = source.ContentPropertyForCollectionMoreThanOneElement;

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForCollectionMoreThanOneElement);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void ContentPropertyForSingleProperty()
        {
            var expectedInstructions = source.ContentPropertyForSingleProperty;

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForSingleMember);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        private ICollection<XamlInstruction> ExtractNodesFromPullParser(string xml)
        {
            var pullParser = new XamlInstructionParser(WiringContext);
            return pullParser.Parse(new XamlProtoInstructionParser(WiringContext).Parse(xml)).ToList();
        }

        [TestMethod]
        public void KeyDirective()
        {
            var expectedInstructions = source.KeyDirective2;

            var actualNodes = ExtractNodesFromPullParser(Dummy.KeyDirective);

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }
    }
}