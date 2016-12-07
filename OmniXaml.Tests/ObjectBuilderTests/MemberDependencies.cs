namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System.Collections.Generic;
    using Model;
    using Xunit;
    
    public class MemberDependencies : ObjectBuilderTestsBase
    {
        [Fact]
        public void DependencyWhenWrongOrder()
        {
            var node = new ConstructionNode(typeof(Setter))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Setter>(control => control.Value),
                        SourceValue = "Value"
                    },
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Setter>(control => control.Property),
                        SourceValue = "NameOfSomeType"
                    }
                }
            };

            var obj = (Setter)Create(node).Result;
            Assert.True(obj.RightOrder);
        }

        [Fact]
        public void DependencyWhenRightOrder()
        {
            var node = new ConstructionNode(typeof(Setter))
            {
                Assignments = new List<MemberAssignment>
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Setter>(control => control.Property),
                        SourceValue = "NameOfSomeType"
                    },
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<Setter>(control => control.Value),
                        SourceValue = "Value"
                    }
                }
            };

            var obj = (Setter)Create(node).Result;
            Assert.True(obj.RightOrder);
        }
    }
}