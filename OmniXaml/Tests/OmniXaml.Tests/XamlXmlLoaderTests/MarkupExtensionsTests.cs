namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Reader.Tests.Wpf;
    using XamlResources = Xaml.Tests.Resources.Dummy;

    [TestClass]
    public class MarkupExtensionsTests : GivenAXamlXmlLoader
    {
        [TestMethod]
        public void SimpleExtension()
        {
            var actualInstance = LoadXaml(XamlResources.SimpleExtension);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Text From Markup Extension", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void SimpleExtensionWithPropertyAssignment()
        {
            var actualInstance = LoadXaml(XamlResources.SimpleExtensionWithOneAssignment);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("SomeValue", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void ExtensionThatRetrievesInteger()
        {
            var actualInstance = LoadXaml("<DummyClass xmlns=\"root\" Number=\"{Int Number=123}\"/>");

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual(123, dummyClass.Number);
        }

        [TestMethod]
        public void QuotedValue()
        {
            var actualInstance = LoadXaml("<DummyClass xmlns=\"root\" SampleProperty=\"{Dummy Property=\'Some Value\'}\"/>");

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Some Value", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void TwoQuotedValues()
        {
            

            var actualInstance = LoadXaml(XamlResources.MarkupExtensionTwoQuotedValues);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Some Value", dummyClass.SampleProperty);
            Assert.AreEqual("Another Value", dummyClass.AnotherProperty);
        }
    }
}
