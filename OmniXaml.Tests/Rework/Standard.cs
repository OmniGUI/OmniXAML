namespace OmniXaml.Tests.Rework
{
    using System.Collections;
    using System.Collections.Generic;
    using Model;
    using OmniXaml.Rework;
    using Xunit;

    public class Standard
    {
        private static void AssertAttachedPropertyCollection(IEnumerable<ConstructionNode> nodes)
        {
            var cn = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Children = nodes,
                        Member = Member.FromAttached<VisualStateManager>("VisualStateGroups")
                    }
                }
            };

            var actual = Create(cn);

            var visualStateGroups = (IEnumerable) VisualStateManager.GetVisualStateGroups(actual);
            Assert.NotEmpty(visualStateGroups);
        }

        private static object Create(ConstructionNode cn)
        {
            return new NewObjectBuilder(new SmartInstanceCreatorMock(), new SmartConverterMock()).Inflate(cn);
        }

        [Fact]
        private void AttachedPropertyThatIsCollectionMultipleElements()
        {
            AssertAttachedPropertyCollection(new[]
            {
                new ConstructionNode(typeof(VisualStateGroup)),
                new ConstructionNode(typeof(VisualStateGroup)),
                new ConstructionNode(typeof(VisualStateGroup))
            });
        }

        [Fact]
        private void AttachedPropertyThatIsCollectionOneElement()
        {
            AssertAttachedPropertyCollection(new[]
            {
                new ConstructionNode(typeof(VisualStateGroup))
            });
        }

        [Fact]
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
            Assert.Equal(new Window {Height = 12}, creationFixture);
        }

        [Fact]
        public void Collection()
        {
            var tree = new ConstructionNode(typeof(Collection))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        SourceValue = "My title",
                        Member = Member.FromStandard<Collection>(collection => collection.Title)
                    }
                },
                Children = new[]
                {
                    new ConstructionNode(typeof(TextBlock)),
                    new ConstructionNode(typeof(TextBlock))
                }
            };

            var actual = Create(tree);

            var expected = new Collection {new TextBlock(), new TextBlock()};
            expected.Title = "My title";

            Assert.Equal(expected, actual);
        }

        [Fact]
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

            var result = (ItemsControl) Create(node);
            Assert.NotNull(result.Items);
            Assert.IsAssignableFrom<IEnumerable>(result.Items);
            Assert.NotEmpty(result.Items);
        }

      

        [Fact]
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
            var result = (ItemsControl) creationFixture;
            Assert.NotNull(result.Items);
            Assert.IsAssignableFrom<IEnumerable>(result.Items);
        }
        
        [Fact]
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

            Assert.Equal(new TextBlock {Text = "MyText"}, b);
        }

        //[Fact]
        //public void LoadInstanceSameType()
        //{
        //    var node = new ConstructionNode(typeof(Window))
        //    {
        //        Assignments = new[]
        //        {
        //            new MemberAssignment
        //            {
        //                Member = Member.FromStandard<Window>(tb => tb.Title),
        //                SourceValue = "My title"
        //            }
        //        }
        //    };

        //    var expected = new Window {Content = "My content"};
        //    var fixture = Create(node, expected);

        //    Assert.True(ReferenceEquals(expected, fixture));
        //    Assert.Equal(new Window {Content = "My content", Title = "My title"}, fixture);
        //}
    }
}