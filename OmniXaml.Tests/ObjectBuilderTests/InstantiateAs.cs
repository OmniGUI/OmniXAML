namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;
    using Model.Custom;

    [TestClass]
    public class InstantiateAs : ObjectBuilderTestsBase
    {
        [TestMethod]
        public void InstantiateAs_Inflates_The_Specified_Class()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                InstantiateAs = typeof(CustomWindow)
            };

            var result = Create(node);
            Assert.IsInstanceOfType(result.Result, typeof(CustomWindow));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidTypeThrows()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                InstantiateAs = typeof(TextBlock)
            };

            Create(node);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MemberOfTheCorrectType()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                InstantiateAs = typeof(TextBlock),
                Assignments = new List<MemberAssignment>()
                {
                    new MemberAssignment
                    {
                        Member = Member.FromStandard<CustomWindow>(window => window.CustomProperty),
                        SourceValue = "SomeValue",
                    }
                }
            };

            var result = Create(node);

            var expected = new CustomWindow() { CustomProperty = "SomeValue" };

            Assert.AreEqual(expected, result.Result);
        }
    }
}