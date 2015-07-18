namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using global::OmniXaml.Parsers.ProtoParser.SuperProtoParser;
    using global::OmniXaml.Parsers.XamlNodes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract class SpecialTests : GivenAWiringContext
    {
        [TestMethod]
        public void LoadWithRootInstance()
        {
            var dummy = new DummyClass
            {
                AnotherProperty = "Other value",
                SampleProperty = "Will be overwritten"
            };

            var loader = new BootstrappableXamlStreamLoader(
                WiringContext,
                new SuperProtoParser(WiringContext),
                new XamlNodesPullParser(WiringContext),
                new DefaultObjectAssemblerFactory(WiringContext));

            var actual = loader.Load("<DummyClass xmlns=\"root\" SampleProperty=\"Value\" />", dummy);

            Assert.IsInstanceOfType(actual, dummy.GetType());
            Assert.AreEqual("Value", ((DummyClass)actual).SampleProperty);
            Assert.AreEqual("Other value", ((DummyClass)actual).AnotherProperty);
        }
    }
}