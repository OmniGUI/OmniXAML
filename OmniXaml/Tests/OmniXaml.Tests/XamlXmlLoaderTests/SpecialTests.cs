namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Assembler;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SpecialTests : GivenAWiringContext
    {
        [TestMethod]
        public void LoadWithRootInstance()
        {
            var dummy = new DummyClass
            {
                AnotherProperty = "Other value",
                SampleProperty = "Will be overwritten"
            };

            var loader = new XamlXmlLoader(new ObjectAssembler(WiringContext, new ObjectAssemblerSettings { RootInstance = dummy }), WiringContext);
            var actual = loader.Load("<DummyClass xmlns=\"root\" SampleProperty=\"Value\" />");

            Assert.IsInstanceOfType(actual, dummy.GetType());
            Assert.AreEqual(((DummyClass)actual).SampleProperty, "Value");
            Assert.AreEqual(((DummyClass)actual).AnotherProperty, "Other value");
        }
    }
}