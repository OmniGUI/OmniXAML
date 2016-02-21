namespace OmniXaml.Tests
{
    using Classes;
    using Classes.WpfLikeModel;
    using Common.DotNetFx;
    using ObjectAssembler;
    using Resources;
    using TypeConversion;
    using Xunit;

    public class NameScopeTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {
        private readonly InstructionResources source;
        private readonly ObjectAssembler sut;

        public NameScopeTests()
        {
            var topDownValueContext = new TopDownValueContext();
            sut = new ObjectAssembler(RuntimeTypeSource, new ValueContext(RuntimeTypeSource, topDownValueContext));
            source = new InstructionResources(this);
        }
       
        [Fact]
        public void RegisterOneChildInNameScope()
        {
            RuntimeTypeSource.ClearNamescopes();
            RuntimeTypeSource.EnableNameScope<DummyClass>();

            sut.Process(source.ChildInNameScope);
            var actual = sut.Result;
            var childInScope = ((DummyObject)actual).Find("MyObject");
            Assert.IsType(typeof(ChildClass), childInScope);
        }


        [Fact]
        public void RegisterChildInDeeperNameScope()
        {
            RuntimeTypeSource.ClearNamescopes();
            RuntimeTypeSource.EnableNameScope<Window>();

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