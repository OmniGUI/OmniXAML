namespace OmniXaml.Tests.Parsers.Integration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Common.DotNetFx;
    using Xunit;
    using OmniXaml.Parsers.Parser;
    using OmniXaml.Parsers.ProtoParser;
    using Resources;
    using Xaml.Tests.Resources;

    public class FromXamlToInstructions : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {
        private readonly InstructionResources source;

        public FromXamlToInstructions()
        {
            source = new InstructionResources(this);
        }

        [Fact]
        public void EmptyString()
        {
            Assert.Throws<XmlException>(() => ExtractNodesFromPullParser(string.Empty));
        }

        [Fact]
        public void SingleInstance()
        {
            var expectedInstructions = source.SingleInstance;

            var actualNodes = ExtractNodesFromPullParser(Dummy.SingleInstance);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }


        [Fact]
        public void RootNamespace()
        {
            var expectedInstructions = source.SingleInstance;

            var actualNodes = ExtractNodesFromPullParser(Dummy.RootNamespace);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }


        [Fact]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var expectedInstructions = source.ObjectWithMember;

            var actualNodes = ExtractNodesFromPullParser(Dummy.StringProperty);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void InstanceWithChild()
        {
            var expectedInstructions = source.InstanceWithChild;

            var actualNodes = ExtractNodesFromPullParser(Dummy.InstanceWithChild);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void DifferentNamespaces()
        {
            var expectedInstructions = source.DifferentNamespaces;

            var actualNodes = ExtractNodesFromPullParser(Dummy.DifferentNamespaces);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }
        
        [Fact]
        public void DifferentNamespacesAndMoreThanOneProperty()
        {
            var expectedInstructions = source.DifferentNamespacesAndMoreThanOneProperty;

            var actualNodes = ExtractNodesFromPullParser(Dummy.DifferentNamespacesAndMoreThanOneProperty);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ClassWithInnerCollection()
        {
            var expectedInstructions = source.CollectionWithOneItem;

            var actualNodes = ExtractNodesFromPullParser(Dummy.ClassWithInnerCollection);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void CollectionWithMoreThanOneItem()
        {
            var expectedInstructions = source.CollectionWithMoreThanOneItem;

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollectionWithMoreThanOneItem);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void CollapsedTagWithProperty()
        {
            var expectedInstructions = source.CollapsedTagWithProperty;

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollapsedTagWithProperty);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }


        [Fact]
        public void CollectionWithClosedItemAndProperty()
        {
            var expectedInstructions = source.CollectionWithOneItemAndAMember;

            var actualNodes = ExtractNodesFromPullParser(Dummy.CollectionWithClosedItemAndProperty);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void SimpleExtension()
        {
            var expectedInstructions = source.SimpleExtension;

            var actualNodes = ExtractNodesFromPullParser(Dummy.SimpleExtension);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void SimpleExtensionWithOneAssignment()
        {
            var expectedInstructions = source.SimpleExtensionWithOneAssignment;

            var actualNodes = ExtractNodesFromPullParser(Dummy.SimpleExtensionWithOneAssignment);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ContentPropertyForCollectionOneElement()
        {
            var expectedInstructions = source.ContentPropertyForCollectionOneElement;

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForCollectionOneElement);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ContentPropertyForCollectionMoreThanOneElement()
        {
            var expectedInstructions = source.ContentPropertyForCollectionMoreThanOneElement;

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForCollectionMoreThanOneElement);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ContentPropertyForSingleProperty()
        {
            var expectedInstructions = source.ContentPropertyForSingleProperty;

            var actualNodes = ExtractNodesFromPullParser(Dummy.ContentPropertyForSingleMember);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        private ICollection<Instruction> ExtractNodesFromPullParser(string xml)
        {
            var pullParser = new InstructionParser(RuntimeTypeSource);

            using (var stream = new StringReader(xml))
            {
                var reader = new XmlCompatibilityReader(stream);
                return pullParser.Parse(new ProtoInstructionParser(RuntimeTypeSource).Parse(reader)).ToList();
            }
        }

        [Fact]
        public void KeyDirective()
        {
            var expectedInstructions = source.KeyDirective2;

            var actualNodes = ExtractNodesFromPullParser(Dummy.KeyDirective);

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }
    }
}