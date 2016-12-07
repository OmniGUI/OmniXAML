namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System.Collections.Generic;
    using Model;
    using Xunit;
    
    public class Events : ObjectBuilderTestsBase
    {
        [Fact]
        public void BasicEvent()
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
                            new ConstructionNode<Button>
                            {
                                Assignments = new[]
                                {
                                    new MemberAssignment
                                    {
                                        Member = Member.FromStandard(typeof(Button), nameof(Button.Click)),
                                        SourceValue = nameof(TestWindow.OnClick)
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var root = new TestWindow();
            Create(node, root);

            (root.Content as Button).ClickButton();

            Assert.True(root.ButtonClicked);
        }

        [Fact]
        public void AttachedEvent()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromAttached(typeof(Window), "Loaded"),
                        SourceValue = nameof(TestWindow.OnLoad)
                    }
                }
            };

            var root = new TestWindow();
            var creationFixture = Create(node, root);

            root.RaiseEvent(new AttachedEventArgs { Event = Window.LoadedEvent });

            Assert.True(root.WindowLoaded);
        }
    }
}