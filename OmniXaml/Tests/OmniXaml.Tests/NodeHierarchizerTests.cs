using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Builder;
    using Classes;
    using Visualization;

    [TestClass]
    public class NodeHierarchizerTests : GivenAWiringContext
    {
        private readonly XamlNodeBuilder x;

        public NodeHierarchizerTests()
        {
            x = new XamlNodeBuilder(WiringContext.TypeContext);
        }

        [TestMethod]
        public void TestMethod1()
        {
            var input = new List<XamlNode>
            {
                x.StartMember<Setter>(c => c.Value),
                x.Value("Value"),
                x.EndMember(),
                x.StartMember<Setter>(c => c.Property),
                x.Value("Property"),
                x.EndMember(),
            };

            var sut = new NodeHierarchizer();
            var actual = sut.CreateHierarchy(input);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var input = new List<XamlNode>
            {
                x.StartMember<Setter>(c => c.Value),
                x.EndMember(),
            };

            var sut = new NodeHierarchizer();
            var actual = sut.CreateHierarchy(input);
            var expected = new Sequence<HierarchizedXamlNode>
            {
                new HierarchizedXamlNode
                {
                    Leading = x.StartMember<Setter>(c => c.Value),
                    Trailing = x.EndMember(),
                }
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestReverseMembers()
        {
            var input = new List<XamlNode>
            {
                x.StartMember<Setter>(c => c.Value),
                    x.StartObject(typeof(DummyClass)),
                        x.StartMember<ChildClass>(c => c.Name),
                            x.Value("Hola"),
                        x.EndMember(),
                        x.StartMember<ChildClass>(c => c.Content),
                            x.Value("Tío"),
                        x.EndMember(),
                    x.EndObject(),
                x.EndMember(),
                x.StartMember<Setter>(c => c.Property),
                    x.StartObject(typeof(DummyClass)),
                        x.StartMember<ChildClass>(c => c.Name),
                            x.Value("Hola"),
                        x.EndMember(),
                        x.StartMember<ChildClass>(c => c.Content),
                            x.Value("Tío"),
                        x.EndMember(),
                    x.EndObject(),
                x.EndMember(),
            };

            var sut = new NodeHierarchizer();
            var actual = sut.CreateHierarchy(input);

            var h = new HierarchizedXamlNode {Children = new Sequence<HierarchizedXamlNode>(actual.ToList())};
            h.AcceptVisitor(new MemberReverserVisitor());

            var actualNodes = h.Children.SelectMany(node => node.Dump());
            var expectedNodes = new List<XamlNode>
            {
                x.StartMember<Setter>(c => c.Property),
                    x.StartObject(typeof(DummyClass)),
                        x.StartMember<ChildClass>(c => c.Content),
                            x.Value("Tío"),
                        x.EndMember(),
                        x.StartMember<ChildClass>(c => c.Name),
                            x.Value("Hola"),
                        x.EndMember(),
                    x.EndObject(),
                x.EndMember(),
                x.StartMember<Setter>(c => c.Value),
                    x.StartObject(typeof(DummyClass)),                       
                        x.StartMember<ChildClass>(c => c.Content),
                            x.Value("Tío"),
                        x.EndMember(),
                        x.StartMember<ChildClass>(c => c.Name),
                            x.Value("Hola"),
                        x.EndMember(),
                    x.EndObject(),
                x.EndMember(),              
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }
    }
}
