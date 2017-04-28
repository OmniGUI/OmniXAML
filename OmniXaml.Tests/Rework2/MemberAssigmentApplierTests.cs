using OmniXaml.Services;

namespace OmniXaml.Tests.Rework2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using Model;
    using ReworkPhases;
    using Xunit;

    public class MemberAssigmentApplierTests
    {
        [Fact]
        public void StandardPropertyAssignmentWithMoreThanOneChild()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var inflatedMemberAssignment = new InflatedMemberAssignment
            {
                Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                Children = new List<InflatedNode>()
                {
                    new InflatedNode()
                    {
                        Instance = "SomeText",
                    },
                    new InflatedNode()
                    {
                        Instance = "SomeText",
                    }
                },

            };

            Assert.Throws<InvalidOperationException>(() => sut.ExecuteAssignment(inflatedMemberAssignment, null));
        }

        [Fact]
        public void StandardPropertyAssignmentOfItemsToCollection()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var inflatedMemberAssignment = new InflatedMemberAssignment
            {
                Member = Member.FromStandard<ItemsControl>(tb => tb.Items),
                Children = new List<InflatedNode>()
                {
                    new InflatedNode
                    {
                        Instance = "A",
                    },
                    new InflatedNode
                    {
                        Instance = "B",
                    }
                },

            };

            var itemsControl = new ItemsControl();

            sut.ExecuteAssignment(inflatedMemberAssignment, itemsControl);

            Assert.Equal(new[] { "A", "B" }, itemsControl.Items);
        }

        [Fact]
        public void StandardPropertyAssignmentWithOneChild()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var textBlock = new TextBlock();
            var inflatedMemberAssignment = new InflatedMemberAssignment
            {
                Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                Children = new List<InflatedNode>()
                {
                    new InflatedNode()
                    {
                        Instance = "SomeText",
                    }
                },

            };

            sut.ExecuteAssignment(inflatedMemberAssignment, textBlock);

            Assert.Equal("SomeText", textBlock.Text);
        }

        [Fact]
        public void AttachedPropertyAssignment()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var textBlock = new TextBlock();
            var inflatedMemberAssignment = new InflatedMemberAssignment
            {
                Member = Member.FromAttached<Grid>("Row"),
                Children = new List<InflatedNode>() { new InflatedNode() { Instance = 1 } }
            };

            sut.ExecuteAssignment(inflatedMemberAssignment, textBlock);

            Assert.Equal(1, Grid.GetRow(textBlock));
        }

        [Fact]
        public void IncompatibleInstances()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var inflatedMemberAssignment = new InflatedMemberAssignment
            {
                Member = Member.FromStandard<Window>(w => w.Height),
                Children = new List<InflatedNode>
                {
                    new InflatedNode
                    {
                        Instance = "12.5",
                    },
                }
            };
            var window = new Window();
            Assert.Throws<ArgumentException>(() => sut.ExecuteAssignment(inflatedMemberAssignment, window));
        }
    }
}