namespace OmniXaml.Tests.Parsers.Integration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Common;
    using Xunit;
    using OmniXaml.Parsers.Parser;
    using OmniXaml.Parsers.ProtoParser;
    using Resources;
    using File = Xaml.Tests.Resources.File;

    public class FromXamlToInstructions : GivenARuntimeTypeSource
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

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\SingleInstance.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }


        [Fact]
        public void RootNamespace()
        {
            var expectedInstructions = source.SingleInstance;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\RootNamespace.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }


        [Fact]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var expectedInstructions = source.ObjectWithMember;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\StringProperty.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void InstanceWithChild()
        {
            var expectedInstructions = source.InstanceWithChild;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\InstanceWithChild.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void DifferentNamespaces()
        {
            var expectedInstructions = source.DifferentNamespaces;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\DifferentNamespaces.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }
        
        [Fact]
        public void DifferentNamespacesAndMoreThanOneProperty()
        {
            var expectedInstructions = source.DifferentNamespacesAndMoreThanOneProperty;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\DifferentNamespacesAndMoreThanOneProperty.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ClassWithInnerCollection()
        {
            var expectedInstructions = source.CollectionWithOneItem;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\ClassWithInnerCollection.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void CollectionWithMoreThanOneItem()
        {
            var expectedInstructions = source.CollectionWithMoreThanOneItem;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\CollectionWithMoreThanOneItem.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void CollapsedTagWithProperty()
        {
            var expectedInstructions = source.CollapsedTagWithProperty;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\CollapsedTagWithProperty.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }


        [Fact]
        public void CollectionWithClosedItemAndProperty()
        {
            var expectedInstructions = source.CollectionWithOneItemAndAMember;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\CollectionWithClosedItemAndProperty.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void SimpleExtension()
        {
            var expectedInstructions = source.SimpleExtension;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\SimpleExtension.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void SimpleExtensionWithOneAssignment()
        {
            var expectedInstructions = source.SimpleExtensionWithOneAssignment;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\SimpleExtensionWithOneAssignment.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ContentPropertyForCollectionOneElement()
        {
            var expectedInstructions = source.ContentPropertyForCollectionOneElement;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\ContentPropertyForCollectionOneElement.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ContentPropertyForCollectionMoreThanOneElement()
        {
            var expectedInstructions = source.ContentPropertyForCollectionMoreThanOneElement;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\ContentPropertyForCollectionMoreThanOneElement.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void ContentPropertyForSingleProperty()
        {
            var expectedInstructions = source.ContentPropertyForSingleProperty;

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\ContentPropertyForSingleMember.xaml"));

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

            var actualNodes = ExtractNodesFromPullParser(File.LoadAsString(@"Xaml\Dummy\KeyDirective.xaml"));

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }
    }
}