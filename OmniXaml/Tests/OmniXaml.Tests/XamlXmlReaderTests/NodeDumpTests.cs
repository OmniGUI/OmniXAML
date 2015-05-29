namespace OmniXaml.Tests.XamlXmlReaderTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Classes;
    using Glass;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.ProtoParser.SuperProtoParser;
    using OmniXaml.Parsers.XamlNodes;
    using Xaml.Tests.Resources;

    [TestClass]
    public class NodeDumpTests
    {
        private readonly XamlNodeBuilder nodeBuilder;
        private readonly WiringContext wiringContext;

        public NodeDumpTests()
        {
            wiringContext = new WiringContextBuilder()
                .AddNsForThisType("", "root", typeof(DummyClass))
                .Build();

            nodeBuilder = new XamlNodeBuilder(wiringContext.TypeContext);
        }

        [TestMethod]
        public void ReadSingleInstance()
        {
            var contents = FlattenNodesFromXaml(Dummy.SingleInstance);

            var expected = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration("root", ""),
                nodeBuilder.StartObject(typeof (DummyClass)),
                nodeBuilder.EndObject(),
            };

            XamlNodesAssert.AreEssentiallyTheSame(expected, contents);
        }

        [TestMethod]
        [Description("With no namespace, it fails")]
        public void ReadWithChild()
        {
            var contents = FlattenNodesFromXaml(Dummy.InstanceWithChild);

            var expected = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration("root", ""),
                nodeBuilder.StartObject(typeof (DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.Child),
                nodeBuilder.StartObject(typeof(ChildClass)),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            XamlNodesAssert.AreEssentiallyTheSame(expected, contents);
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var contents = FlattenNodesFromXaml(Dummy.StringProperty);

            var expected = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration("root", ""),
                nodeBuilder.StartObject(typeof (DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.SampleProperty),
                nodeBuilder.Value("Property!"),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            XamlNodesAssert.AreEssentiallyTheSame(expected, contents);
        }

        [TestMethod]
        public void ClassWithInnerCollection()
        {
            var contents = FlattenNodesFromXaml(Dummy.ClassWithInnerCollection);

            var expected = new List<XamlNode>
            {
                nodeBuilder.NamespacePrefixDeclaration("root", ""),
                nodeBuilder.StartObject(typeof (DummyClass)),
                nodeBuilder.StartMember<DummyClass>(d => d.Items),
                nodeBuilder.GetObject(),
                nodeBuilder.Items(),
                nodeBuilder.StartObject(typeof (Item)),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
                nodeBuilder.EndMember(),
                nodeBuilder.EndObject(),
            };

            XamlNodesAssert.AreEssentiallyTheSame(expected, contents);
        }

        [TestMethod]
        [Ignore]
        public void ReadRealXamlStage1()
        {
            var actualNodes = FlattenNodesFromXaml(Dummy.XamlStage1);
            var expectedNodes = ParseResult(Dummy.XamlStage1_Result);

            AssertNodesAreEqual(expectedNodes, actualNodes.Select(node => node.NodeType).ToList());
        }

        [TestMethod]
        [Ignore]
        public void ReadRealXamlStage2()
        {
            var actualNodes = FlattenNodesFromXaml(Dummy.XamlStage2);
            var expectedNodes = ParseResult(Dummy.XamlStage2_Result);

            AssertNodesAreEqual(expectedNodes, actualNodes.Select(node => node.NodeType).ToList());
        }

        private static void AssertNodesAreEqual(IList<XamlNodeType> expectedNodes, IList<XamlNodeType> actualNodes)
        {
            CollectionAssert.AreEqual(
                expectedNodes.ToList(),
                actualNodes.ToList(),
                "\nExpected:\n{0}\n\nActual:\n{1}",
                Extensions.ToString(expectedNodes),
                Extensions.ToString(actualNodes));
        }

        private static IList<XamlNodeType> ParseResult(string str)
        {
            var nodes = new List<XamlNodeType>();

            var nodeTypes = Enum.GetNames(typeof(XamlNodeType));

            using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(str))))
            {
                var builder = new StringBuilder();

                while (!reader.EndOfStream)
                {
                    var readChar = (char)reader.Read();
                    builder.Append(readChar);
                    var currentBuffer = builder.ToString();

                    var nodeType = nodeTypes.FirstOrDefault(nodeName => nodeName == currentBuffer);

                    if (nodeType != null)
                    {
                        XamlNodeType parsedNode;
                        if (Enum.TryParse(nodeType, out parsedNode))
                        {
                            nodes.Add(parsedNode);
                            builder.Clear();
                        }
                    }
                }
            }

            return nodes;
        }

        private IList<XamlNode> FlattenNodesFromXaml(string xaml)
        {
            var pullParser = new XamlNodesPullParser(wiringContext);
            var protoNodes = new SuperProtoParser(wiringContext).Parse(xaml);
            return pullParser.Parse(protoNodes).ToList();         
        }
    }
}