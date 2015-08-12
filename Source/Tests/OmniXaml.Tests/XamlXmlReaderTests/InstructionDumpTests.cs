namespace OmniXaml.Tests.XamlXmlReaderTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Classes;
    using Common;
    using Glass;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlNodes;
    using Xaml.Tests.Resources;

    [TestClass]
    public class InstructionDumpTests : GivenAWiringContextWithNodeBuilders
    {
        [TestMethod]
        public void ReadSingleInstance()
        {
            var contents = ParseInstructions(Dummy.SingleInstance);

            var expected = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof (DummyClass)),
                X.EndObject(),
            };

            XamlNodesAssert.AreEssentiallyTheSame(expected, contents);
        }

        [TestMethod]
        [Description("With no namespace, it fails")]
        public void ReadWithChild()
        {
            var contents = ParseInstructions(Dummy.InstanceWithChild);

            var expected = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof (DummyClass)),
                X.StartMember<DummyClass>(d => d.Child),
                X.StartObject(typeof(ChildClass)),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            XamlNodesAssert.AreEssentiallyTheSame(expected, contents);
        }

        [TestMethod]
        public void InstanceWithStringPropertyAndNsDeclaration()
        {
            var contents = ParseInstructions(Dummy.StringProperty);

            var expected = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof (DummyClass)),
                X.StartMember<DummyClass>(d => d.SampleProperty),
                X.Value("Property!"),
                X.EndMember(),
                X.EndObject(),
            };

            XamlNodesAssert.AreEssentiallyTheSame(expected, contents);
        }

        [TestMethod]
        public void ClassWithInnerCollection()
        {
            var contents = ParseInstructions(Dummy.ClassWithInnerCollection);

            var expected = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(RootNs),
                X.StartObject(typeof (DummyClass)),
                X.StartMember<DummyClass>(d => d.Items),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof (Item)),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            XamlNodesAssert.AreEssentiallyTheSame(expected, contents);
        }      

        private static void AssertNodesAreEqual(IList<XamlNodeType> expectedInstructions, IList<XamlNodeType> actualNodes)
        {
            CollectionAssert.AreEqual(
                expectedInstructions.ToList(),
                actualNodes.ToList(),
                "\nExpected:\n{0}\n\nActual:\n{1}",
                Extensions.ToString(expectedInstructions),
                Extensions.ToString(actualNodes));
        }

        private static IList<XamlNodeType> ParseResult(string str)
        {
            var nodes = new List<XamlNodeType>();

            var nodeTypes = Enum.GetNames(typeof(XamlNodeType));

            using (var reader = new StreamReader(str.ToStream()))
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

        private IList<XamlInstruction> ParseInstructions(string xaml)
        {
            var pullParser = new XamlInstructionParser(WiringContext);
            var protoNodes = new XamlProtoInstructionParser(WiringContext).Parse(xaml);
            return pullParser.Parse(protoNodes).ToList();         
        }
    }
}