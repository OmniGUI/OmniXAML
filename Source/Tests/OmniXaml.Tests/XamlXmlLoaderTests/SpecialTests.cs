namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Common.DotNetFx;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract class SpecialTests : GivenARuntimeTypeSourceNetCore
    {
        [TestMethod]
        public void LoadWithRootInstance()
        {
            var dummy = new DummyClass
            {
                AnotherProperty = "Other value",
                SampleProperty = "Will be overwritten"
            };

            var loader = new XmlLoader(new DummyParserFactory(RuntimeTypeSource));

            var actual = loader.FromString("<DummyClass xmlns=\"root\" SampleProperty=\"Value\" />", dummy);

            Assert.IsInstanceOfType(actual, dummy.GetType());
            Assert.AreEqual("Value", ((DummyClass)actual).SampleProperty);
            Assert.AreEqual("Other value", ((DummyClass)actual).AnotherProperty);
        }
    }
}