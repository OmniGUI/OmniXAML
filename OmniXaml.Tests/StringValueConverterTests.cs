using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests
{
    public class StringValueConverterTests
    {
        [Theory]
        [InlineData("hola", "hola")]
        [InlineData("1", 1)]
        [InlineData("1.4", 1.4)]
        [InlineData("1.6", 1.6F)]
        [InlineData("Wrap", TextWrapping.Wrap)]
        public void Convert(string sourceValue, object converted)
        {
            var sut = CreateSut();
            Assert.Equal((true, converted), sut.Convert(sourceValue, converted.GetType()));
        }
      
        private static StringValueConverter CreateSut()
        {
            return new StringValueConverter();
        }
    }
}
