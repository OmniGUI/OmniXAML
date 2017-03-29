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
                UnresolvedAssignments = new HashSet<UnresolvedMemberAssignment>(new List<UnresolvedMemberAssignment>()
                {
                    new UnresolvedMemberAssignment
                    {
                        Member = Member.FromStandard<TextBlock>(tb => tb.Text),
                        Children = new List<UnresolvedNode>
                        {
                            new UnresolvedNode
                            {
                                SourceValue = "Pepito",
                            },
                        },
                    },
                })
            };

            var actual = sut.Resolve(inflatedNode);

            Assert.Equal(expected, actual);
        }
    }
}