namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using System;
    using System.Xml;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Reader.Tests.Wpf;
    using Xaml.Tests.Resources;

    [TestClass]
    public class SingleInstantiationTests : GivenAXamlXmlLoader
    {
        readonly Type expectedType = typeof(DummyClass);

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void EmptyStreamThrows()
        {
            XamlStreamLoader.Load(Dummy.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(TypeNotFoundException))]
        public void UnknownElementThrows()
        {
            XamlStreamLoader.Load(Dummy.UnknownType);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void BadFormatThrowsXamlReaderException()
        {
            XamlStreamLoader.Load(Dummy.BadFormat);
        }

        [TestMethod]
        public void NoPrefixMapsToNamespaceAndReturnsTheCorrectInstance()
        {
            var actual = XamlStreamLoader.Load(Dummy.RootNamespace);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void SimpleXamlWithCollapsedTagsShouldReadLikeExplicitEndingTag()
        {
            var actual = XamlStreamLoader.Load(Dummy.CollapsedTag);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void DifferentNamespacesShouldReturnCorrectInstances()
        {
            var actual = XamlStreamLoader.Load(Dummy.DifferentNamespaces);
            Assert.IsInstanceOfType(actual, expectedType);
        }       
    }
}
