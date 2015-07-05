namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Assembler;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SpecialTestsClassicObjectAssembler : SpecialTestsBase
    {
        protected override IXamlLoader CreateLoaderWithRootInstance(DummyClass dummy)
        {
            return new XamlXmlLoader(new ObjectAssembler(WiringContext, new ObjectAssemblerSettings { RootInstance = dummy }), WiringContext);
        }
    }
}