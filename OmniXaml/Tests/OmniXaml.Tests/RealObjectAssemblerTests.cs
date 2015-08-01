namespace OmniXaml.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NewAssembler;

    [TestClass]
    public class RealObjectAssemblerTests : ObjectAssemblerTests
    {
        protected override IObjectAssembler CreateObjectAssembler()
        {
            return new SuperObjectAssembler(WiringContext, new TopDownMemberValueContext());
        }
    }
}