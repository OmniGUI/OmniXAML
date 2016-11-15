namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;
    using OmniXaml.Ambient;

    [TestClass]
    public class Ambient : ObjectBuilderTestsBase
    {
        [TestMethod]
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

            CollectionAssert.AreEqual(assigments, result.BuildContext.AmbientRegistrator.Assigments.ToList());
        }

        [TestMethod]
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

            CollectionAssert.AreEqual(instances, result.BuildContext.AmbientRegistrator.Instances.ToList());
        }

        [TestMethod]
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

            CollectionAssert.AreEqual(assigments, result.BuildContext.AmbientRegistrator.Assigments.ToList());
        }
    }
}