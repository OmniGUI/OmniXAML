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
            BoostrappableXamlStreamLoader.Load(Dummy.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(TypeNotFoundException))]
        public void UnknownElementThrows()
        {
            BoostrappableXamlStreamLoader.Load(Dummy.UnknownType);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void BadFormatThrowsXamlReaderException()
        {
            BoostrappableXamlStreamLoader.Load(Dummy.BadFormat);
        }

        [TestMethod]
        public void NoPrefixMapsToNamespaceAndReturnsTheCorrectInstance()
        {
            var actual = BoostrappableXamlStreamLoader.Load(Dummy.RootNamespace);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void SimpleXamlWithCollapsedTagsShouldReadLikeExplicitEndingTag()
        {
            var actual = BoostrappableXamlStreamLoader.Load(Dummy.CollapsedTag);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void DifferentNamespacesShouldReturnCorrectInstances()
        {
            var actual = BoostrappableXamlStreamLoader.Load(Dummy.DifferentNamespaces);
            Assert.IsInstanceOfType(actual, expectedType);
        }       
    }
}
