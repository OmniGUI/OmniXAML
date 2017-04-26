namespace OmniXaml.Tests.Rework2
{
    using System;
    using System.Collections.Generic;
    using Model;
    using ReworkPhases;
    using Services;
    using Xunit;

    public class MemberApplierTests
    {
        [Fact]
        public void ApplyTest()
        {
            var sut = new MemberAssigmentApplier(new FuncStringConverterExtended((s, type) => (true, Convert.ChangeType(s, type))), new NoActionValuePipeline());
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
            var success= sut.TryApply(inflatedMemberAssignment, textBlock);

            Assert.True(success);
            Assert.Equal("SomeText", textBlock.Text);
        }
    }
}