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
            Loader.Load(Dummy.Empty);
        }

        [TestMethod]        
        [ExpectedException(typeof(TypeNotFoundException))]
        public void UnknownElementThrows()
        {
            Loader.Load(Dummy.UnknownType);
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void BadFormatThrowsXamlReaderException()
        {
            Loader.Load(Dummy.BadFormat);
        }

        [TestMethod]
        public void NoPrefixMapsToNamespaceAndReturnsTheCorrectInstance()
        {
            var actual = Loader.Load(Dummy.RootNamespace);
            Assert.IsInstanceOfType(actual, expectedType);
        }
     
        [TestMethod]
        public void SimpleXamlWithCollapsedTagsShouldReadLikeExplicitEndingTag()
        {
            var actual = Loader.Load(Dummy.CollapsedTag);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void DifferentNamespacesShouldReturnCorrectInstances()
        {
            var actual = Loader.Load(Dummy.DifferentNamespaces);
            Assert.IsInstanceOfType(actual, expectedType);
        }
    }
}
