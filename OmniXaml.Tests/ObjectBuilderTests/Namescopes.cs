namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class Namescopes : ObjectBuilderTestsBase
    {
        [TestMethod]
        public void Namescope()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(tb => tb.Content),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode<TextBlock> {Name = "MyTextBlock"}
                        }
                    }
                }
            };

            var actual = Create(node);
            var textBlock = actual.BuildContext.NamescopeAnnotator.Find("MyTextBlock", actual.Result);
            Assert.IsInstanceOfType(textBlock, typeof(TextBlock));
        }

        [TestMethod]
        public void NamescopeLevel()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(tb => tb.Content),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode<ItemsControl>
                            {
                                Assignments = new List<MemberAssignment>
                                {
                                    new MemberAssignment
                                    {
                                        Member = Member.FromStandard<ItemsControl>(c => c.Items),
                                        Children = new ConstructionNode[]
                                        {
                                            new ConstructionNode<TextBlock>
                                            {
                                                Name = "One"
                                            },
                                            new ConstructionNode<TextBlock>
                                            {
                                                Name = "Two"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var actual = Create(node);
            var one = actual.BuildContext.NamescopeAnnotator.Find("One", actual.Result);
            var two = actual.BuildContext.NamescopeAnnotator.Find("Two", actual.Result);
            Assert.IsInstanceOfType(one, typeof(TextBlock));
            Assert.IsInstanceOfType(two, typeof(TextBlock));
        }
    }
}