namespace OmniXaml.Tests.ObjectBuilderTests
{
    using System;
    using System.Collections.Generic;
    using Model;
    using Model.Custom;
    using Xunit;

    public class InstantiateAs : ObjectBuilderTestsBase
    {
        [Fact]
        public void InstantiateAs_Inflates_The_Specified_Class()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                InstantiateAs = typeof(CustomWindow)
            };

            var result = Create(node);
            Assert.IsType<CustomWindow>(result.Result);
        }

        [Fact]
        public void InvalidTypeThrows()
        {
            var node = new ConstructionNode(typeof(Window))
            {
                InstantiateAs = typeof(TextBlock)
            };

            Assert.Throws<InvalidOperationException>(() => Create(node));
        }        
    }
}