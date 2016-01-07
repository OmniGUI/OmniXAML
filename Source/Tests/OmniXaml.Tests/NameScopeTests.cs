namespace OmniXaml.Tests
{
    using Classes;
    using Classes.WpfLikeModel;
    using Common.NetCore;
    using ObjectAssembler;
    using Resources;
    using Xunit;

    public class NameScopeTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {
        private readonly InstructionResources source;
        private readonly ObjectAssembler sut;

        public NameScopeTests()
        {
            sut = new ObjectAssembler(TypeRuntimeTypeSource, new TopDownValueContext());
            source = new InstructionResources(this);
        }
       
        [Fact]
        public void RegisterOneChildInNameScope()
        {
            TypeRuntimeTypeSource.ClearNamescopes();
            TypeRuntimeTypeSource.EnableNameScope<DummyClass>();

            sut.Process(source.ChildInNameScope);
            var actual = sut.Result;
            var childInScope = ((DummyObject)actual).Find("MyObject");
            Assert.IsType(typeof(ChildClass), childInScope);
        }


        [Fact]
        public void RegisterChildInDeeperNameScope()
        {
            TypeRuntimeTypeSource.ClearNamescopes();
            TypeRuntimeTypeSource.EnableNameScope<Window>();

            sut.Process(source.ChildInDeeperNameScope);
            var actual = sut.Result;
            var textBlock1 = ((Window)actual).Find("MyTextBlock");
            var textBlock2 = ((Window)actual).Find("MyOtherTextBlock");

            Assert.IsType(typeof(TextBlock), textBlock1);
            Assert.IsType(typeof(TextBlock), textBlock2);
        }

        [Fact]
        public void NameWithNoNamescopesToRegisterTo()
        {
            sut.Process(source.NameWithNoNamescopesToRegisterTo);
        }
    }
}