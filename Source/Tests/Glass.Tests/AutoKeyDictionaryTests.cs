namespace Glass.Tests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AutoKeyDictionaryTests
    {
        [TestMethod]
        public void IndexerTestKeyExist()
        {
            var sut = new AutoKeyDictionary<int, string>(i => i + 1, i => i < 3);
            var value = "Pepito";
            sut.Add(2, value);
            var result = sut[1];
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void IndexerTestKeyDoesNotExist()
        {
            var sut = new AutoKeyDictionary<int?, string>(i => i + 1, i => i < 2);
            var result = sut[1];
        }

        [TestMethod]
        public void TryGetTestKeyExist()
        {
            var sut = new AutoKeyDictionary<int, string>(i => i + 1, i => i < 3);
            var expected = "Pepito";
            sut.Add(2, expected);
            string actual;
            var result = sut.TryGetValue(1, out actual);
            Assert.AreEqual(expected, actual);
        }
    }
}