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
    public class NodeHierarchizerTests : GivenAWiringContextWithNodeBuilders
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
                X.StartMember<Setter>(c => c.Value),
                X.Value("Value"),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Property),
                X.Value("Property"),
                X.EndMember(),
            };

            var sut = new NodeHierarchizer();
            var actual = sut.CreateHierarchy(input);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var input = new List<XamlNode>
            {
                X.StartMember<Setter>(c => c.Value),
                X.EndMember(),
            };

            var sut = new NodeHierarchizer();
            var actual = sut.CreateHierarchy(input);
            var expected = new Sequence<HierarchizedXamlNode>
            {
                new HierarchizedXamlNode
                {
                    Leading = X.StartMember<Setter>(c => c.Value),
                    Trailing = X.EndMember(),
                }
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestReverseMembers()
        {
            var input = new List<XamlNode>
            {
                X.StartMember<Setter>(c => c.Value),
                    X.StartObject(typeof(DummyClass)),
                        X.StartMember<ChildClass>(c => c.Name),
                            X.Value("Hola"),
                        X.EndMember(),
                        X.StartMember<ChildClass>(c => c.Content),
                            X.Value("Tío"),
                        X.EndMember(),
                    X.EndObject(),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Property),
                    X.StartObject(typeof(DummyClass)),
                        X.StartMember<ChildClass>(c => c.Name),
                            X.Value("Hola"),
                        X.EndMember(),
                        X.StartMember<ChildClass>(c => c.Content),
                            X.Value("Tío"),
                        X.EndMember(),
                    X.EndObject(),
                X.EndMember(),
            };

            var sut = new NodeHierarchizer();
            var actual = sut.CreateHierarchy(input);

            var h = new HierarchizedXamlNode {Children = new Sequence<HierarchizedXamlNode>(actual.ToList())};
            h.AcceptVisitor(new MemberReverserVisitor());

            var actualNodes = h.Children.SelectMany(node => node.Dump());
            var expectedNodes = new List<XamlNode>
            {
                X.StartMember<Setter>(c => c.Property),
                    X.StartObject(typeof(DummyClass)),
                        X.StartMember<ChildClass>(c => c.Content),
                            X.Value("Tío"),
                        X.EndMember(),
                        X.StartMember<ChildClass>(c => c.Name),
                            X.Value("Hola"),
                        X.EndMember(),
                    X.EndObject(),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Value),
                    X.StartObject(typeof(DummyClass)),                       
                        X.StartMember<ChildClass>(c => c.Content),
                            X.Value("Tío"),
                        X.EndMember(),
                        X.StartMember<ChildClass>(c => c.Name),
                            X.Value("Hola"),
                        X.EndMember(),
                    X.EndObject(),
                X.EndMember(),              
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }

        [TestMethod]
        public void DependencySorting()
        {
            var input = new List<XamlNode>
            {
                X.StartMember<Setter>(c => c.Value),
                X.Value("Value"),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Property),
                X.Value("Property"),
                X.EndMember(),
            };

            var sut = new NodeHierarchizer();
            var actual = sut.CreateHierarchy(input);

            var h = new HierarchizedXamlNode { Children = new Sequence<HierarchizedXamlNode>(actual.ToList()) };
            h.AcceptVisitor(new DependencySortingVisitor());

            var actualNodes = h.Children.SelectMany(node => node.Dump());
            var expectedNodes = new List<XamlNode>
            {
                X.StartMember<Setter>(c => c.Property),
                X.Value("Property"),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Value),
                X.Value("Value"),
                X.EndMember(),
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes.ToList());
        }
    }
}
