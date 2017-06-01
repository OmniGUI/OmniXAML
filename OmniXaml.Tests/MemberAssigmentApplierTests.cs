using System;
using System.Collections.Generic;
using OmniXaml.Services;
using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests
{
    public class MemberAssigmentApplierTests
    {
        [Fact]
        public void StandardPropertyAssignmentWithMoreThanOneChild()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var nodes = new List<ConstructionNode>
            {
                new ConstructionNode()
                {
                    Instance = "SomeText",
                },
                new ConstructionNode
                {
                    Instance = "SomeText",
                }
            };

            var assignment = new MemberAssignment
            {
                Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                Values = nodes,
            };

            Assert.Throws<InvalidOperationException>(() => sut.ExecuteAssignment(new NodeAssignment(assignment, null), null, null));
        }

        [Fact]
        public void StandardPropertyAssignmentOfItemsToCollection()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var nodes = new List<ConstructionNode>()
            {
                new ConstructionNode
                {
                    Instance = "A",
                },
                new ConstructionNode
                {
                    Instance = "B",
                }
            };
            var assignment = new MemberAssignment
            {
                Member = Member.FromStandard<ItemsControl>(tb => tb.Items),
                Values = nodes,
            };

            var itemsControl = new ItemsControl();

            sut.ExecuteAssignment(new NodeAssignment(assignment, itemsControl), null, null);

            Assert.Equal(new[] { "A", "B" }, itemsControl.Items);
        }

        [Fact]
        public void StandardPropertyAssignmentWithOneChild()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var textBlock = new TextBlock();
            var nodes = new List<ConstructionNode>
            {
                new ConstructionNode
                {
                    Instance = "SomeText",
                }
            };
            var assignment = new MemberAssignment
            {
                Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                Values = nodes,
            };

            sut.ExecuteAssignment(new NodeAssignment(assignment, textBlock), null, null);

            Assert.Equal("SomeText", textBlock.Text);
        }

        [Fact]
        public void AttachedPropertyAssignment()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var textBlock = new TextBlock();
            var nodes = new List<ConstructionNode>() { new ConstructionNode() { Instance = 1 } };
            var assignment = new MemberAssignment
            {
                Member = Member.FromAttached<Grid>("Row"),
                Values = nodes,
            };

            sut.ExecuteAssignment(new NodeAssignment(assignment, textBlock), null, null);

            Assert.Equal(1, Grid.GetRow(textBlock));
        }

        [Fact]
        public void IncompatibleInstances()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var nodes = new List<ConstructionNode>
            {
                new ConstructionNode
                {
                    Instance = "12.5",
                },
            };
            var assignment = new MemberAssignment
            {
                Member = Member.FromStandard<Window>(w => w.Height),
                Values=nodes
            };
            var window = new Window();
            Assert.Throws<ArgumentException>(() => sut.ExecuteAssignment(new NodeAssignment(assignment, window), null, null));
        }
    }
}