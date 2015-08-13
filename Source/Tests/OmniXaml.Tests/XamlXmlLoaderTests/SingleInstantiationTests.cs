namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using System;
    using System.Xml;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xaml.Tests.Resources;

    [TestClass]
    public class SingleInstantiationTests : GivenAXamlXmlLoader
    {
        readonly Type expectedType = typeof(DummyClass);

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void EmptyStreamThrows()
        {
            XamlLoader.Load(Dummy.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(XamlParsingException))]
        public void UnknownElementThrows()
        {
            XamlLoader.Load(Dummy.UnknownType);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void BadFormatThrowsXamlReaderException()
        {
            XamlLoader.Load(Dummy.BadFormat);
        }

        [TestMethod]
        public void NoPrefixMapsToNamespaceAndReturnsTheCorrectInstance()
        {
            var actual = XamlLoader.Load(Dummy.RootNamespace);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void SimpleXamlWithCollapsedTagsShouldReadLikeExplicitEndingTag()
        {
            var actual = XamlLoader.Load(Dummy.CollapsedTag);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void DifferentNamespacesShouldReturnCorrectInstances()
        {
            var actual = XamlLoader.Load(Dummy.DifferentNamespaces);
            Assert.IsInstanceOfType(actual, expectedType);
        }       
    }
}
