using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AberratioReader.Tests
{
    [TestClass]
    public class SingleInstantiationTests
    {
        private XamlReaderBuilder xamlReaderBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            xamlReaderBuilder = new XamlReaderBuilder();
        }

        [TestMethod]
        [ExpectedException(typeof(XamlReaderException))]
        public void EmptyStreamThrowsXamlReadingException()
        {            
            var sut = xamlReaderBuilder.Build();
            LoadFromString(sut, Xaml.Empty);
        }

        [TestMethod]
        public void SingleInstanceReturnsInstanceOfThatType()
        {
            XamlReaderBuilder builder = new XamlReaderBuilder();
            var sut = builder.Build();
            var actual = LoadFromString(sut, Xaml.SingleInstance);

            Assert.IsInstanceOfType(actual, typeof(DummyClass));
        }

        private static object LoadFromString(XamlReader sut, string contents)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents)))
            {
                return sut.Load(stream);
            }
        }
    }
}
