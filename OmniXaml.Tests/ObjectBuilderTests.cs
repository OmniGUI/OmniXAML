namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Ambient;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class ObjectBuilderTests
    {
        [TestMethod]
        public void TemplateContent()
        {
            var node = new ConstructionNode(typeof(ItemsControl))
            {
                Assignments = new List<PropertyAssignment>
                {
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty<ItemsControl>(control => control.ItemTemplate),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode(typeof(DataTemplate))
                            {
                                Assignments = new[]
                                {
                                    new PropertyAssignment
                                    {
                                        Property = Property.RegularProperty<DataTemplate>(template => template.Content),
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
                    new List<PropertyAssignment>
                    {
                        new PropertyAssignment
                        {
                            Property = Property.RegularProperty<SimpleExtension>(extension => extension.Property),
                            SourceValue = "MyText"
                        }
                    }
            };

            var node = new ConstructionNode(typeof(TextBlock))
            {
                Assignments = new[]
                {
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty<TextBlock>(tb => tb.Text),
                        Children = new[] {constructionNode}
                    }
                }
            };

            var b = Create(node);

            Assert.AreEqual(new TextBlock { Text = "MyText" }, b.ResultingObject);
        }

        [TestMethod]
        public void GivenExtensionThatProvidesCollection_TheCollectionIsProvided()
        {
            var extensionNode = new ConstructionNode(typeof(CollectionExtension));

            var node = new ConstructionNode(typeof(ItemsControl))
            {
                Assignments = new[]
                {
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty<ItemsControl>(tb => tb.Items),
                        Children = new[] {extensionNode}
                    }
                }
            };

            var creationFixture = Create(node);
            var result = (ItemsControl)creationFixture.ResultingObject;
            Assert.IsNotNull(result.Items);
            Assert.IsInstanceOfType(result.Items, typeof(IEnumerable));
        }

        [TestMethod]
        public void Collection()
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
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty<ItemsControl>(tb => tb.Items),
                        Children = items
                    }
                }
            };

            var result = (ItemsControl)Create(node).ResultingObject;
            Assert.IsNotNull(result.Items);
            Assert.IsInstanceOfType(result.Items, typeof(IEnumerable));
            Assert.IsTrue(result.Items.Any());
        }

        [Ignore]
        [TestMethod]
        public void ImmutableFromContent()
        {
            var node = new ConstructionNode(typeof(MyImmutable)) { InjectableArguments = new[] { "Hola" } };
            var myImmutable = new MyImmutable("Hola");
            var fixture = Create(node);

            Assert.AreEqual(myImmutable, fixture.ResultingObject);
        }

        [TestMethod]
        public void LoadInstanceSameType()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty<Window>(tb => tb.Title),
                        SourceValue = "My title"
                    }
                }
            };

            var expected = new Window { Content = "My content" };
            var fixture = Create(node, expected);

            Assert.IsTrue(ReferenceEquals(expected, fixture.ResultingObject));
            Assert.AreEqual(new Window { Content = "My content", Title = "My title" }, fixture.ResultingObject);
        }

        [TestMethod]
        public void Namescope()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty<Window>(tb => tb.Content),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode<TextBlock> {Name = "MyTextBlock"}
                        }
                    }
                }
            };

            var actual = Create(node);
            var textBlock = actual.TrackingContext.Annotator.Find("MyTextBlock", actual.ResultingObject);
            Assert.IsInstanceOfType(textBlock, typeof(TextBlock));
        }

        [TestMethod]
        public void AmbientDirectValue()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty<Window>(tb => tb.Content),
                        SourceValue = "Hello",
                    }
                }
            };

            var result = Create(node);
            var assigments = new[] { new AmbientPropertyAssignment { Property = Property.RegularProperty<Window>(window => window.Content), Value = "Hello" }, };

            CollectionAssert.AreEqual(assigments, result.TrackingContext.AmbientRegistrator.Assigments.ToList());
        }

        [TestMethod]
        public void AmbientInnerNode()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty<Window>(tb => tb.Content),
                        Children = new List<ConstructionNode>() { new ConstructionNode(typeof(TextBlock))},
                    }
                }
            };

            var result = Create(node);
            var assigments = new[] { new AmbientPropertyAssignment { Property = Property.RegularProperty<Window>(window => window.Content), Value = new TextBlock() }, };

            CollectionAssert.AreEqual(assigments, result.TrackingContext.AmbientRegistrator.Assigments.ToList());
        }

        [TestMethod]
        public void NamescopeLevel()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new PropertyAssignment
                    {
                        Property = Property.RegularProperty<Window>(tb => tb.Content),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode<ItemsControl>
                            {
                                Assignments = new List<PropertyAssignment>
                                {
                                    new PropertyAssignment
                                    {
                                        Property = Property.RegularProperty<ItemsControl>(c => c.Items),
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
            var one = actual.TrackingContext.Annotator.Find("One", actual.ResultingObject);
            var two = actual.TrackingContext.Annotator.Find("Two", actual.ResultingObject);
            Assert.IsInstanceOfType(one, typeof(TextBlock));
            Assert.IsInstanceOfType(two, typeof(TextBlock));
        }

        private CreationFixture Create(ConstructionNode node, object rootInstance)
        {
            var constructionContext = new ConstructionContext(
                new InstanceCreator(),
                new SourceValueConverter(),
                Context.GetMetadataProvider());

            var builder = new ExtendedObjectBuilder(
                constructionContext,
                (assignment, context) => new MarkupExtensionContext(assignment, constructionContext, new TypeDirectory()));

            var creationContext = new TrackingContext(new NamescopeAnnotator(), new AmbientRegistrator(), new InstanceLifecycleSignaler());
            return new CreationFixture
            {
                ResultingObject = builder.Create(node, rootInstance, creationContext),
                TrackingContext = creationContext
            };
        }

        private static CreationFixture Create(ConstructionNode node)
        {
            var constructionContext = new ConstructionContext(
                new InstanceCreator(),
                new SourceValueConverter(),
                Context.GetMetadataProvider());

            var builder = new ExtendedObjectBuilder(
                constructionContext,
                (assignment, context) => new MarkupExtensionContext(assignment, constructionContext, new TypeDirectory()));

            var creationContext = new TrackingContext(new NamescopeAnnotator(), new AmbientRegistrator(), new InstanceLifecycleSignaler());
            return new CreationFixture
            {
                ResultingObject = builder.Create(node, creationContext),
                TrackingContext = creationContext
            };
        }
    }
}