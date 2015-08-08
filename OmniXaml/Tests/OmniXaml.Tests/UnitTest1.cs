namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Classes.WpfLikeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Typing;
    using Visualization;

    [TestClass]
    public class OrderAwareXamlNodesPullParserTests : GivenAWiringContextWithNodeBuilders
    {
        [TestMethod]
        public void FilterOutput()
        {
            var input = new List<XamlNode>
            {
                X.StartObject<Style>(),
                    X.StartMember<Style>(c => c.Setter),
                        X.StartObject<Setter>(),
                            X.StartMember<Setter>(c => c.Value),
                                X.Value("Value"),
                            X.EndMember(),
                            X.StartMember<Setter>(c => c.Property),
                                X.Value("Property"),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject()
            };

            var actualNodes = FilterInput(input.GetEnumerator()).ToList();
            var expectedNodes = new List<XamlNode>
            {
                X.StartObject<Style>(),
                    X.StartMember<Style>(c => c.Setter),
                        X.StartObject<Setter>(),
                            X.StartMember<Setter>(c => c.Property),
                                X.Value("Property"),
                            X.EndMember(),
                            X.StartMember<Setter>(c => c.Value),
                                X.Value("Value"),
                            X.EndMember(),
                        X.EndObject(),
                    X.EndMember(),
                X.EndObject()
            };

            CollectionAssert.AreEqual(expectedNodes, actualNodes);
        }

        [TestMethod]
        public void LookAheadTest()
        {
            var look = new List<XamlNode>
            {
                X.StartObject<Style>(),
                X.StartMember<Setter>(c => c.Value),
                X.Value("Value"),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Property),
                X.Value("Property"),
                X.EndMember(),
                X.EndObject(),
            };

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = LookAhead(enumerator).Count();
            Assert.AreEqual(8, count);
        }

        [TestMethod]
        public void LookAheadTestStartZero()
        {
            var look = new List<XamlNode>();

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = LookAhead(enumerator).Count();
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void LookAheadTestStartMiniumLength()
        {
            var look = new List<XamlNode>
            {
                X.StartObject<Style>(),                
                X.EndObject()
            };

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = LookAhead(enumerator).Count();
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void LookAheadTest10()
        {
            var look = new List<XamlNode>
            {
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.StartObject<Setter>(),
                X.EndObject(),
                X.EndObject(),
                X.EndObject(),
                X.EndObject(),
                X.EndObject(),
            };

            var enumerator = look.GetEnumerator();
            enumerator.MoveNext();
            var count = LookAhead(enumerator).Count();
            Assert.AreEqual(10, count);
        }

        private IEnumerable<XamlNode> FilterInput(IEnumerator<XamlNode> enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.NodeType == XamlNodeType.StartObject)
                {
                    var hasDeps = GetSomeMemberHasDependencies(enumerator.Current.XamlType);
                    if (hasDeps)
                    {
                        foreach (var xamlNode in SortNodes(enumerator)) yield return xamlNode;
                    }

                }

                yield return enumerator.Current;

            }
        }

        private IEnumerable<XamlNode> SortNodes(IEnumerator<XamlNode> enumerator)
        {
            var subSet = LookAhead(enumerator).ToList();
            var nodes = new NodeHierarchizer().CreateHierarchy(subSet);
            var root = new HierarchizedXamlNode { Children = new Sequence<HierarchizedXamlNode>(nodes.ToList()) };
            root.AcceptVisitor(new DependencySortingVisitor());

            foreach (var xamlNode in root.Children.SelectMany(node => node.Dump()))
            {
                yield return xamlNode;
            }
        }

        private static IEnumerable<XamlNode> LookAhead(IEnumerator<XamlNode> enumerator)
        {
            var count = 0;
            var yielded = 0;
            var isEndOfOffendingBlock = false;

            if (enumerator.Current.Equals(default(XamlNode)))
            {
                yield break;
            }

            do
            {


                yield return enumerator.Current;
                yielded++;

                if (enumerator.Current.NodeType == XamlNodeType.StartObject)
                {
                    count++;
                }
                else if (enumerator.Current.NodeType == XamlNodeType.EndObject)
                {
                    count--;
                }

                if (count == 0)
                {
                    isEndOfOffendingBlock = true;
                }

                enumerator.MoveNext();

            } while (!isEndOfOffendingBlock);
        }

        private static bool GetSomeMemberHasDependencies(XamlType xamlType)
        {
            var allMembers = xamlType.GetAllMembers().OfType<MutableXamlMember>();
            return allMembers.Any(member => member.Dependencies.Any());
        }
    }
}
