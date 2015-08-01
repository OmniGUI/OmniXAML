namespace OmniXaml.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NewAssembler;

    [TestClass]
    public class SuperObjectAssemblerTests : ObjectAssemblerTests
    {
        protected override IObjectAssembler CreateObjectAssembler()
        {
            return new SuperObjectAssembler(WiringContext, new TopDownMemberValueContext());
        }     
    }
}