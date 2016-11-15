namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;
    using Model.Custom;

    [TestClass]
    public class Standard : ObjectBuilderTestsBase
    {
        [TestMethod]
        public void TemplateContent()
        {
            var node = new ConstructionNode(typeof(ItemsControl))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<ItemsControl>(control => control.ItemTemplate),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode(typeof(DataTemplate))
                            {
                                Assignments = new[]
                                {
                                    new MemberAssignment
                                    {
                                        Member = Member.FromStandard<DataTemplate>(template => template.Content),
                                        Children = new[] {new ConstructionNode(typeof(TextBlock))}
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var obj = Create(node);
        }

        [TestMethod]
        public void GivenSimpleExtensionThatProvidesAString_TheStringIsProvided()
        {
            var constructionNode = new ConstructionNode(typeof(SimpleExtension))
            {
                Assignments =
                    new List<MemberAssignment>
                    {
                        new MemberAssignment
                        {
                            Member = Member.FromStandard<SimpleExtension>(extension => extension.Property),
                            SourceValue = "MyText"
                        }
                    }
            };

            var node = new ConstructionNode(typeof(TextBlock))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                        Children = new[] {constructionNode}
                    }
                }
            };

            var b = Create(node);

            Assert.AreEqual(new TextBlock {Text = "MyText"}, b.Result);
        }

        [TestMethod]
        public void GivenExtensionThatProvidesCollection_TheCollectionIsProvided()
        {
            var extensionNode = new ConstructionNode(typeof(CollectionExtension));

            var node = new ConstructionNode(typeof(ItemsControl))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<ItemsControl>(tb => tb.Items),
                        Children = new[] {extensionNode}
                    }
                }
            };

            var creationFixture = Create(node);
            var result = (ItemsControl) creationFixture.Result;
            Assert.IsNotNull(result.Items);
            Assert.IsInstanceOfType(result.Items, typeof(IEnumerable));
        }

        [TestMethod]
        public void CollectionProperty()
        {
            var items = new[]
            {
                new ConstructionNode(typeof(TextBlock)),
                new ConstructionNode(typeof(TextBlock)),
                new ConstructionNode(typeof(TextBlock))
            };

            var node = new ConstructionNode(typeof(ItemsControl))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<ItemsControl>(tb => tb.Items),
                        Children = items
                    }
                }
            };

            var result = (ItemsControl) Create(node).Result;
            Assert.IsNotNull(result.Items);
            Assert.IsInstanceOfType(result.Items, typeof(IEnumerable));
            Assert.IsTrue(result.Items.Any());
        }

        [TestMethod]
        public void ImmutableFromContent()
        {
            var node = new ConstructionNode(typeof(MyImmutable)) {InjectableArguments = new[] {"Hola"}};
            var myImmutable = new MyImmutable("Hola");
            var fixture = Create(node);

            Assert.AreEqual(myImmutable, fixture.Result);
        }

        [TestMethod]
        public void ParametrizedExtension()
        {
            var node = new ConstructionNode(typeof(ParametrizedExtension)) {InjectableArguments = new[] {"Hola"}};
            var myImmutable = new ParametrizedExtension("Hola");
            var fixture = Create(node);

            Assert.AreEqual(myImmutable, fixture.Result);
        }

        [TestMethod]
        public void LoadInstanceSameType()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(tb => tb.Title),
                        SourceValue = "My title"
                    }
                }
            };

            var expected = new Window {Content = "My content"};
            var fixture = Create(node, expected);

            Assert.IsTrue(ReferenceEquals(expected, fixture.Result));
            Assert.AreEqual(new Window {Content = "My content", Title = "My title"}, fixture.Result);
        }

    

        [TestMethod]
        public void BasicProperty()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(tb => tb.Height),
                        SourceValue = "12"
                    }
                }
            };

            var creationFixture = Create(node);
            Assert.AreEqual(new Window {Height = 12}, creationFixture.Result);
        }

       

    




        [TestMethod]
        public void Collection()
        {
            var tree = new ConstructionNode(typeof(Collection))
            {
                Assignments = new[]
                {
                    new MemberAssignment {SourceValue = "My title", Member = Member.FromStandard<Collection>(collection => collection.Title)}
                },
                Children = new[]
                {
                    new ConstructionNode(typeof(TextBlock))
                }
            };

            var actual = Create(tree).Result;

            var expected = new Collection {new TextBlock()};
            expected.Title = "My title";

            Assert.AreEqual(expected, actual);
        }
    }
}