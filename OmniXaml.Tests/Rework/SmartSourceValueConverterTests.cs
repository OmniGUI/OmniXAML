namespace OmniXaml.Tests.Rework
{
    using Model;
    using OmniXaml.Rework;
    using Xunit;

    public class SmartSourceValueConverterTests
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
            Assert.Equal((true, converted), sut.TryConvert(sourceValue, converted.GetType()));
        }
      
        private static SmartSourceValueConverter CreateSut()
        {
            return new SmartSourceValueConverter();
        }
    }
}
