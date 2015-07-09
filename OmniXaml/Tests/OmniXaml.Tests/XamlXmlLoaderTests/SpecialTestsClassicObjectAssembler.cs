namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Assembler;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NewAssembler;

    [TestClass]
    public class SpecialTestsClassicObjectAssembler : SpecialTestsBase
    {
        protected override IXamlLoader CreateLoaderWithRootInstance(DummyClass dummy)
        {
            return new XamlXmlLoader(new SuperObjectAssembler(WiringContext, new ObjectAssemblerSettings { RootInstance = dummy }), WiringContext);
        }
    }
}