using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OmniXaml.ReworkPhases;
using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests
{
    public class TreeFixerTests
    {
        [Fact]
        public void FixNode()
        {
            var tree = new InflatedNode()
            {
                Instance = new TextBlock(),
                Assignments = new List<InflatedMemberAssignment>
                {
                    new InflatedMemberAssignment
                    {
                        Member = Member.FromStandard<TextBlock>(block => block.Text),
                        Values = new List<InflatedNode>
                        {
                            new InflatedNode
                            {
                                Instance = null,
                                SourceValue = "Hola",
                                ConversionFailed = true,
                            }
                        }
                    }
                }
            };

            var sut = new ObjectBuilderSecondPass(new FuncStringConverter(s => (true, s)), new FuncAssignmentApplier(
                (assignment, o) =>
                {
                    o.GetType().GetProperty(assignment.Member.MemberName)
                        .SetValue(o, assignment.Values.First().Instance);
                }));

            sut.Fix(tree);
        }
    }
}