namespace OmniXaml.Tests
{
    using Classes;
    using Classes.WpfLikeModel;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectAssembler;
    using Resources;

    [TestClass]
    public class NameScopeTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private readonly XamlInstructionResources source;
        private readonly ObjectAssembler sut;

        public NameScopeTests()
        {
            sut = new ObjectAssembler(WiringContext, new TopDownValueContext());
            source = new XamlInstructionResources(this);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            WiringContext.ClearNamesCopes();
        }

        [TestMethod]
        public void RegisterOneChildInNameScope()
        {
            WiringContext.EnableNameScope<DummyClass>();

            sut.PumpNodes(source.ChildInNameScope);
            var actual = sut.Result;
            var childInScope = ((DummyObject)actual).Find("MyObject");
            Assert.IsInstanceOfType(childInScope, typeof(ChildClass));
        }


        [TestMethod]
        public void RegisterChildInDeeperNameScope()
        {
            WiringContext.EnableNameScope<Window>();

            sut.PumpNodes(source.ChildInDeeperNameScope);
            var actual = sut.Result;
            var textBlock1 = ((Window)actual).Find("MyTextBlock");
            var textBlock2 = ((Window)actual).Find("MyOtherTextBlock");

            Assert.IsInstanceOfType(textBlock1, typeof(TextBlock));
            Assert.IsInstanceOfType(textBlock2, typeof(TextBlock));
        }

        [TestMethod]
        public void NameWithNoNamescopesToRegisterTo()
        {
            sut.PumpNodes(source.NameWithNoNamescopesToRegisterTo);
        }
    }
}