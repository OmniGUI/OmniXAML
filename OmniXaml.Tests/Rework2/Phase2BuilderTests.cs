namespace OmniXaml.Tests.Rework2
{
    using System.Collections.Generic;
    using Model;
    using Rework;
    using ReworkPhases;
    using Xunit;

    public class Phase2BuilderTests
    {
        [Fact]
        public void SingleInstance()
        {
            var smartSourceValueConverter = new SmartConverterMock();
            smartSourceValueConverter.SetConvertFunc((s, type) => (true, s));
            var sut = new Phase2Builder(smartSourceValueConverter);
            var expected = new TextBlock()
            {
                Text = "Pepito",
            };

            var inflatedNode = new InflatedNode
            {
                Instance = new TextBlock(),
                UnresolvedAssignments = new HashSet<InflatedMemberAssignment>(new List<InflatedMemberAssignment>()
                {
                    new InflatedMemberAssignment
                    {
                        Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                        Children = new List<InflatedNode>
                        {
                            new InflatedNode
                            {
                                SourceValue = "Pepito",
                            },
                        },
                    },
                })
            };

            var actual = sut.Fix(inflatedNode);

            Assert.Equal(expected, actual);
        }
    }
}