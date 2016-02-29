namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Common.DotNetFx;
    using Xunit;
    using Resources;

    public class InstructionTreeBuilderTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {
        private readonly InstructionResources source;

        public InstructionTreeBuilderTests()
        {
            source = new InstructionResources(this);
        }

        [Fact]
        public void InstructionToNodeConversionWithLeadingAndTrailing()
        {
            var input = new List<Instruction>
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

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InstructionToNodeConversionWithLeadingBodyAndTrailing()
        {
            var input = new List<Instruction>
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

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestReverseMembers()
        {
            var input = source.TestReverseMembers;

            var sut = new InstructionTreeBuilder();
            var actual = sut.CreateHierarchy(input);

            var h = new InstructionNode { Children = new Sequence<InstructionNode>(actual.ToList()) };
            h.AcceptVisitor(new MemberReverserVisitor());

            var actualNodes = h.Children.SelectMany(node => node.Dump());
            var expectedInstructions = source.TestReverseMembersReverted;

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void TestReverseMembersWithCollection()
        {
            var input = source.TestReverseMembers;

            var sut = new InstructionTreeBuilder();
            var actual = sut.CreateHierarchy(input);

            var h = new InstructionNode { Children = new Sequence<InstructionNode>(actual.ToList()) };
            h.AcceptVisitor(new MemberReverserVisitor());

            var actualNodes = h.Children.SelectMany(node => node.Dump());
            var expectedInstructions = source.TestReverseMembersReverted;

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }

        [Fact]
        public void DependencySorting()
        {
            var input = source.TwoMembers;

            var sut = new InstructionTreeBuilder();
            var actual = sut.CreateHierarchy(input);

            var h = new InstructionNode { Children = new Sequence<InstructionNode>(actual.ToList()) };
            h.AcceptVisitor(new DependencySortingVisitor());

            var actualNodes = h.Children.SelectMany(node => node.Dump());
            var expectedInstructions = source.TwoMembersReversed;

            Assert.Equal(expectedInstructions.ToList(), actualNodes.ToList());
        }


        [Fact]
        public void GivenCollectionAndSimpleMember_AfterCreatingHierarchy_DumpReturnsTheOriginalInput()
        {
            var sut = new InstructionTreeBuilder();

            var instructionNodes = sut.CreateHierarchy(source.ComboBoxCollectionOnly.ToList());

            var nodes = instructionNodes.SelectMany(node => node.Dump()).ToList();

            Assert.Equal(source.ComboBoxCollectionOnly.ToList(), nodes);
        }
    }
}
