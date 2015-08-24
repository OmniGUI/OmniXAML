namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract class SpecialTests : GivenAWiringContextNetCore
    {
        [TestMethod]
        public void LoadWithRootInstance()
        {
            var dummy = new DummyClass
            {
                AnotherProperty = "Other value",
                SampleProperty = "Will be overwritten"
            };

            var loader = new XamlLoader(new DummyXamlParserFactory(WiringContext));

            var actual = loader.Load("<DummyClass xmlns=\"root\" SampleProperty=\"Value\" />", dummy);

            Assert.IsInstanceOfType(actual, dummy.GetType());
            Assert.AreEqual("Value", ((DummyClass)actual).SampleProperty);
            Assert.AreEqual("Other value", ((DummyClass)actual).AnotherProperty);
        }
    }
}