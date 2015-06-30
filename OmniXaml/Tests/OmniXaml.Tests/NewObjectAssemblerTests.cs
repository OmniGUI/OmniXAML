namespace OmniXaml.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NewAssembler;

    [TestClass]
    public class NewObjectAssemblerTests : ObjectAssemblerTests
    {
        protected override IObjectAssembler CreateNewObjectAssembler()
        {
            return new SuperObjectAssembler(WiringContext);
        }
    }    
}