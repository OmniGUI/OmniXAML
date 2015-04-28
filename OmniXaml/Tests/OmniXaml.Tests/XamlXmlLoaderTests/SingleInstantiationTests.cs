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
        readonly Type expectedType;
        
        public SingleInstantiationTests()
        {
            expectedType = typeof(DummyClass);        
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void EmptyStreamThrows()
        {
            LoadXaml(Dummy.Empty);
        }

        [TestMethod]        
        [ExpectedException(typeof(XamlReaderException))]
        public void UnknownElementThrows()
        {
            LoadXaml(Dummy.UnknownType);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void BadFormatThrowsXamlReaderException()
        {
            LoadXaml(Dummy.BadFormat);
        }

        [TestMethod]
        public void NoPrefixMapsToNamespaceAndReturnsTheCorrectInstance()
        {
            var actual = LoadXaml(Dummy.RootNamespace);
            Assert.IsInstanceOfType(actual, expectedType);
        }
     
        [TestMethod]
        public void SimpleXamlWithCollapsedTagsShouldReadLikeExplicitEndingTag()
        {
            var actual = LoadXaml(Dummy.CollapsedTag);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void DifferentNamespacesShouldReturnCorrectInstances()
        {
            var actual = LoadXaml(Dummy.DifferentNamespaces);
            Assert.IsInstanceOfType(actual, expectedType);
        }
    }
}
