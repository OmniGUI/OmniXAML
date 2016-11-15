namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class MemberDependencies : ObjectBuilderTestsBase
    {
        [TestMethod]
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
            Assert.IsTrue(obj.RightOrder);
        }

        [TestMethod]
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
            Assert.IsTrue(obj.RightOrder);
        }
    }
}