namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Builder;
    using Classes;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Resources;

    [TestClass]
    public class NodeHierarchizerTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private readonly XamlInstructionResources source;

        public NodeHierarchizerTests()
        {
            source = new XamlInstructionResources(this);
        }

        [TestMethod]
        public void InstructionToNodeConversionWithLeadingAndTrailing()
        {
            var input = new List<XamlInstruction>
            {
                X.StartMember<Setter>(c => c.Value),
                X.EndMember(),
            };

            var sut = new InstructionTreeBuilder();
            var actual = sut.CreateHierarchy(input).ToList();
            var expected = new Sequence<InstructionNode>
            {
                new InstructionNode
                {
                    Leading = X.StartMember<Setter>(c => c.Value),
                    Trailing = X.EndMember(),
                }
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InstructionToNodeConversionWithLeadingBodyAndTrailing()
        {
            var input = new List<XamlInstruction>
            {
                X.StartMember<Setter>(c => c.Value),
                X.Value("Value"),
                X.EndMember(),
                X.StartMember<Setter>(c => c.Property),
                X.Value("Property"),
                X.EndMember(),
            };

            var sut = new InstructionTreeBuilder();
            var actual = sut.CreateHierarchy(input).ToList();

            var expected = new Collection<InstructionNode>
            {
                new InstructionNode
                {
                    Leading = X.StartMember<Setter>(setter => setter.Value),
                    Body = { X.Value( "Value") },
                    Trailing = X.EndMember(),
                },
                new InstructionNode
                {
                    Leading = X.StartMember<Setter>(setter => setter.Property),
                    Body = { X.Value( "Property") },
                    Trailing = X.EndMember(),
                }
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestReverseMembers()
        {
            var input = source.TestReverseMembers;

            var sut = new InstructionTreeBuilder();
            var actual = sut.CreateHierarchy(input);

            var h = new InstructionNode { Children = new Sequence<InstructionNode>(actual.ToList()) };
            h.AcceptVisitor(new MemberReverserVisitor());

            var actualNodes = h.Children.SelectMany(node => node.Dump());
            var expectedInstructions = source.TestReverseMembersReverted;

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [TestMethod]
        public void DependencySorting()
        {
            var input = source.TwoMembers;

            var sut = new InstructionTreeBuilder();
            var actual = sut.CreateHierarchy(input);

            var h = new InstructionNode { Children = new Sequence<InstructionNode>(actual.ToList()) };
            h.AcceptVisitor(new DependencySortingVisitor());

            var actualNodes = h.Children.SelectMany(node => node.Dump());
            var expectedInstructions = source.TwoMembersReversed;

            CollectionAssert.AreEqual(expectedInstructions.ToList(), actualNodes.ToList());
        }
    }
}
