namespace OmniXaml.Tests
{
    using Builder;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Typing;

    [TestClass]
    public class XamlTypeRepositoryTests
    {
        private readonly Mock<IXamlNamespaceRegistry> nsRegistryMock;

        public XamlTypeRepositoryTests()
        {
            nsRegistryMock = new Mock<IXamlNamespaceRegistry>();

            var type = typeof(DummyClass);

            var fullyConfiguredMapping = Namespace
                .CreateMapFor(type.Namespace)
                .FromAssembly(type.Assembly)
                .To("root");

            nsRegistryMock.Setup(registry => registry.GetXamlNamespace("root"))
                .Returns(fullyConfiguredMapping);
        }

        [TestMethod]
        public void GetWithFullAddressReturnsCorrectType()
        {          
            var sut = new XamlTypeRepository(nsRegistryMock.Object);

            var xamlType = sut.GetWithFullAddress(new XamlTypeName("root", "DummyClass"));

            Assert.IsFalse(xamlType.IsUnreachable);
            Assert.AreEqual(xamlType.UnderlyingType, typeof(DummyClass));
        }


        [TestMethod]
        public void FullAddressOfUnknownReturnUnreachable()
        {
            var sut = new XamlTypeRepository(nsRegistryMock.Object);

            const string UnreachableTypeName = "UnreachableType";

            var xamlType = sut.GetWithFullAddress(new XamlTypeName("root", UnreachableTypeName));

            Assert.IsTrue(xamlType.IsUnreachable);
            Assert.AreEqual(UnreachableTypeName, xamlType.Name);
        }
    }
}
