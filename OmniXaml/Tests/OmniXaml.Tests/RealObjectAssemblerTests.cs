namespace OmniXaml.Tests
{
    using Assembler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RealObjectAssemblerTests : ObjectAssemblerTests
    {
        protected override IObjectAssembler CreateObjectAssembler()
        {
            return new ObjectAssembler(WiringContext);
        }
    }
}