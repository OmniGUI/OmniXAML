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
            var sut = new MemberAssigmentApplier(new FuncStringConverterExtended((s, type) => (true, s)), new NoActionValuePipeline());
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

            Assert.Throws<InvalidOperationException>(() => sut.TryApply(inflatedMemberAssignment, null));
        }

        [Fact]
        public void StandardPropertyAssignmentOfItemsToCollection()
        {
            var sut = new MemberAssigmentApplier(new FuncStringConverterExtended((s, type) => (true, s)), new NoActionValuePipeline());
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

            sut.TryApply(inflatedMemberAssignment, itemsControl);

            Assert.Equal(new[] {"A", "B"}, itemsControl.Items);
        }

        [Fact]
        public void StandardPropertyAssignmentWithOneChild()
        {
            var sut = new MemberAssigmentApplier(new FuncStringConverterExtended((s, type) => (true, s)), new NoActionValuePipeline());
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
            var success = sut.TryApply(inflatedMemberAssignment, textBlock);

            Assert.True(success);
            Assert.Equal("SomeText", textBlock.Text);
        }

        [Fact]
        public void ConvertFail()
        {
            var sut = new MemberAssigmentApplier(new FuncStringConverterExtended((s, t) => (false, null)), new NoActionValuePipeline());
            var inflatedMemberAssignment = new InflatedMemberAssignment
            {
                Member = Member.FromStandard<Window>(w => w.Height),
                Children = new List<InflatedNode>()
                {
                    new InflatedNode()
                    {
                        Instance = "12.5",
                    },
                }
            };
            var window = new Window();
            var success = sut.TryApply(inflatedMemberAssignment, window);

            Assert.False(success);            
        }
    }
}