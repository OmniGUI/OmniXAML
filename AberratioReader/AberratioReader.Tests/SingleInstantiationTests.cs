using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AberratioReader.Tests
{
    [TestClass]
    public class SingleInstantiationTests
    {
        [TestMethod]
        [ExpectedException(typeof(XamlReaderException))]
        public void EmptyStreamThrowsXamlReadingException()
        {
            XamlReaderBuilder builder = new XamlReaderBuilder();
            var sut = builder.Build();
            LoadFromString(sut, Xaml.Empty);
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
