namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract class SpecialTestsBase : GivenAWiringContext
    {
        protected abstract IXamlLoader CreateLoaderWithRootInstance(DummyClass dummy);

        [TestMethod]
        public void LoadWithRootInstance()
        {
            var dummy = new DummyClass
            {
                AnotherProperty = "Other value",
                SampleProperty = "Will be overwritten"
            };

            var loader = CreateLoaderWithRootInstance(dummy);
            var actual = loader.Load("<DummyClass xmlns=\"root\" SampleProperty=\"Value\" />");

            Assert.IsInstanceOfType(actual, dummy.GetType());
            Assert.AreEqual("Value", ((DummyClass)actual).SampleProperty);
            Assert.AreEqual("Other value", ((DummyClass)actual).AnotherProperty);
        }
    }
}