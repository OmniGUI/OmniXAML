namespace OmniXaml.Tests
{
    using System;
    using Assembler;
    using Glass;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NewAssembler;
    using Typing;

    [TestClass]
    public class NewObjectAssemblerTests : ObjectAssemblerTests
    {
        protected override IObjectAssembler CreateNewObjectAssembler()
        {
            return new SuperObjectAssembler(WiringContext);
        }
    }    
}