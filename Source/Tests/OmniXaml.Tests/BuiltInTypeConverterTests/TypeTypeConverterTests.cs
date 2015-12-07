namespace OmniXaml.Tests.BuiltInTypeConverterTests
{
    using System.Globalization;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TypeConversion;
    using TypeConversion.BuiltInConverters;
    using Typing;

    [TestClass]
    public class TypeTypeConverterTests
    {
        [TestMethod]
        public void ConvertFromIdentifiableStringReturnsTheCorrectType()
        {
            var typeContextMock = new Mock<ITypeContext>();
            var typeRepoMock = new Mock<IXamlTypeRepository>();

            var typeLocator = "type locator";

            var xamlType = new XamlType(typeof (DummyClass), typeRepoMock.Object, new TypeFactoryDummy(), new TypeFeatureProviderDummy());
            typeContextMock.Setup(context => context.GetByQualifiedName(It.Is<string>(s => s.Equals(typeLocator))))
                .Returns(xamlType);

            var contextMock = new Mock<IXamlTypeConverterContext>();
            contextMock.Setup(context => context.TypeRepository).Returns(typeContextMock.Object);


            var sut = new TypeTypeConverter();
            var t = sut.ConvertFrom(contextMock.Object, CultureInfo.CurrentCulture, typeLocator);
            Assert.AreEqual(typeof(DummyClass), t);
        }
    }
}
