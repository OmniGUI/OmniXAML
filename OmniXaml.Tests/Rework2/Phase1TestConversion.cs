namespace OmniXaml.Tests.Rework2
{
    using System.Collections.Generic;
    using System.Globalization;
    using Model;
    using Rework;
    using ReworkPhases;
    using Xunit;

    public class Phase1TestConversion
    {
        [Fact]
        public void ConvertTest()
        {
            var ctn = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Window>(window => window.Height),
                        Children = new List<ConstructionNode>
                        {
                            new ConstructionNode(typeof(string)) { SourceValue = "12.5"}
                        }
                    }
                }
            };

            var inflatedNode = new InflatedNode
            {
                Instance = new Window(),
                UnresolvedAssignments = new HashSet<InflatedMemberAssignment>
                {
                    new InflatedMemberAssignment
                    {
                        Member = Member.FromStandard<Window>(w => w.Height),
                        Children = new List<InflatedNode>
                        {
                            new InflatedNode
                            {
                                Instance = 12.5
                            }
                        }
                    }
                }
            };

            Assert.Equal(inflatedNode, CreateSut().Inflate(ctn), new InflatedNodeComparer());
        }

        private static Phase1Builder CreateSut()
        {
            var converter = new SmartConverterMock();
            converter.SetConvertFunc((str, type) => (true, double.Parse(str, CultureInfo.InvariantCulture)));
            return new Phase1Builder(new SimpleInstanceCreator(), converter);
        }
    }
}