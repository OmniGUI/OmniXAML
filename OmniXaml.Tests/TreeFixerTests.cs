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
            var textBlock = new TextBlock();

            var tree = new InflatedNode
            {
                Instance = textBlock,                
            }.WithAssignments(new List<InflatedMemberAssignment>
            {
                new InflatedMemberAssignment
                {
                    Member = Member.FromStandard<TextBlock>(block => block.Text),                    
                }.WithValues(new List<InflatedNode>
                {
                    new InflatedNode
                    {
                        Instance = null,
                        SourceValue = "Hola",
                        IsPendingCreate = true,
                    }
                })
            });

            var sut = new ObjectBuilderSecondPass(new FuncStringConverter(s => (true, s)), new FuncAssignmentApplier(
                (assignment, o) =>
                {
                    o.GetType().GetProperty(assignment.Member.MemberName)
                        .SetValue(o, assignment.Values.First().Instance);
                }));

            sut.Fix(tree);

            Assert.Equal("Hola", ((TextBlock)tree.Instance).Text);
        }
    }
}