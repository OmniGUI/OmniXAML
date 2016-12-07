namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using OmniXaml.Ambient;
    using Xunit;

    public class Ambient : ObjectBuilderTestsBase
    {
        [Fact]
        public void AmbientDirectValue()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(tb => tb.Content),
                        SourceValue = "Hello"
                    }
                }
            };

            var result = Create(node);
            var assigments = new[] { new AmbientMemberAssignment { Property = Member.FromStandard<Window>(window => window.Content), Value = "Hello" } };

            Assert.Equal(assigments, result.BuildContext.AmbientRegistrator.Assigments.ToList());
        }

        [Fact]
        public void AmbientInstances()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(tb => tb.Content),
                        SourceValue = "Hello"
                    }
                }
            };

            var result = Create(node);
            var instances = new[] { new Window { Content = "Hello" } };

            Assert.Equal(instances, result.BuildContext.AmbientRegistrator.Instances.ToList());
        }

        [Fact]
        public void AmbientInnerNode()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                Assignments = new[]
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(tb => tb.Content),
                        Children = new List<ConstructionNode> {new ConstructionNode(typeof(TextBlock))}
                    }
                }
            };

            var result = Create(node);
            var assigments = new[] { new AmbientMemberAssignment { Property = Member.FromStandard<Window>(window => window.Content), Value = new TextBlock() } };

            Assert.Equal(assigments, result.BuildContext.AmbientRegistrator.Assigments.ToList());
        }
    }
}