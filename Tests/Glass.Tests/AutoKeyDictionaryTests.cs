namespace Glass.Tests
{
    using System.Collections.Generic;
    using Xunit;

    public class AutoKeyDictionaryTests
    {
        [Fact]
        public void IndexerTestKeyExist()
        {
            var sut = new AutoKeyDictionary<int, string>(i => i + 1, i => i < 3);
            var value = "Pepito";
            sut.Add(2, value);
            var result = sut[1];
            Assert.Equal(value, result);
        }

        [Fact]
        public void IndexerTestKeyDoesNotExist()
        {
            var sut = new AutoKeyDictionary<int?, string>(i => i + 1, i => i < 2);
            Assert.Throws<KeyNotFoundException>(() => sut[1]);
        }

        [Fact]
        public void TryGetTestKeyExist()
        {
            var sut = new AutoKeyDictionary<int, string>(i => i + 1, i => i < 3);
            var expected = "Pepito";
            sut.Add(2, expected);
            string actual;
            var result = sut.TryGetValue(1, out actual);
            Assert.Equal(expected, actual);
        }
    }
}