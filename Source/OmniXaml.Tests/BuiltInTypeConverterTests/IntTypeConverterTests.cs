namespace OmniXaml.Tests.BuiltInTypeConverterTests
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Moq;
    using TypeConversion;
    using TypeConversion.BuiltInConverters;
    using Xunit;
    using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    public class IntTypeConverterTests
    {
        private Mock<IXamlTypeConverterContext> contextMock;
        private IntTypeConverter sut;

        public IntTypeConverterTests()
        {
            var typeContextMock = new Mock<ITypeContext>();

            contextMock = new Mock<IXamlTypeConverterContext>();
            contextMock.Setup(context => context.TypeRepository).Returns(typeContextMock.Object);
            sut = new IntTypeConverter();
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("-3", -3)]
        [InlineData(4L, 4)]
        public void ConvertFromTests(object value, int converted)
        {
            
            var actual = sut.ConvertFrom(contextMock.Object, CultureInfo.CurrentCulture, value);
            Assert.AreEqual(converted, actual);
        }

        [Theory]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(int), true)]
        [InlineData(typeof(long), true)]
        [InlineData(typeof(bool), false)]
        [InlineData(typeof(Assembly), false)]
        public void CanConvertFromTests(Type type, bool canConvert)
        {

            var actual = sut.CanConvertFrom(null, type);
            Assert.AreEqual(canConvert, actual);
        }
    }
}