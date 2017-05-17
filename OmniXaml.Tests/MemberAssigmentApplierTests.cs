using System;
using System.Collections.Generic;
using OmniXaml.ReworkPhases;
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
            var inflatedMemberAssignment = new InflatedMemberAssignment
            {
                Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                

            }.WithValues(new List<InflatedNode>
            {
                new InflatedNode
                {
                    Instance = "SomeText",
                },
                new InflatedNode
                {
                    Instance = "SomeText",
                }
            });

            Assert.Throws<InvalidOperationException>(() => sut.ExecuteAssignment(inflatedMemberAssignment, null));
        }

        [Fact]
        public void StandardPropertyAssignmentOfItemsToCollection()
        {
            var sut = new MemberAssigmentApplier(new NoActionValuePipeline());
            var inflatedMemberAssignment = new InflatedMemberAssignment
            {
                Member = Member.FromStandard<ItemsControl>(tb => tb.Items),              
            }.WithValues(new List<InflatedNode>()
            {
                new InflatedNode
                {
                    Instance = "A",
                },
                new InflatedNode
                {
                    Instance = "B",
                }
            });

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


            }.WithValues(new List<InflatedNode>
            {
                new InflatedNode
                {
                    Instance = "SomeText",
                }
            });

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
            }.WithValues(new List<InflatedNode>() { new InflatedNode() { Instance = 1 } });

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
            }.WithValues(new List<InflatedNode>
            {
                new InflatedNode
                {
                    Instance = "12.5",
                },
            });
            var window = new Window();
            Assert.Throws<ArgumentException>(() => sut.ExecuteAssignment(inflatedMemberAssignment, window));
        }
    }
}