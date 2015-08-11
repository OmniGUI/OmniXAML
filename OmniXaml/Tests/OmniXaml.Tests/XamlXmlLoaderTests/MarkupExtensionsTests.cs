namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using XamlResources = Xaml.Tests.Resources.Dummy;

    [TestClass]
    public class MarkupExtensionsTests : GivenAXamlXmlLoader
    {
        [TestMethod]
        public void SimpleExtension()
        {
            var actualInstance = XamlLoader.Load(XamlResources.SimpleExtension);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Text From Markup Extension", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void SimpleExtensionWithPropertyAssignment()
        {
            var actualInstance = XamlLoader.Load(XamlResources.SimpleExtensionWithOneAssignment);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("SomeValue", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void ExtensionThatRetrievesInteger()
        {
            var actualInstance = XamlLoader.Load("<DummyClass xmlns=\"root\" Number=\"{Int Number=123}\"/>");

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual(123, dummyClass.Number);
        }

        [TestMethod]
        public void QuotedValue()
        {
            var actualInstance = XamlLoader.Load("<DummyClass xmlns=\"root\" SampleProperty=\"{Dummy Property=\'Some Value\'}\"/>");

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Some Value", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void TwoQuotedValues()
        {
            

            var actualInstance = XamlLoader.Load(XamlResources.MarkupExtensionTwoQuotedValues);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Some Value", dummyClass.SampleProperty);
            Assert.AreEqual("Another Value", dummyClass.AnotherProperty);
        }
    }
}
