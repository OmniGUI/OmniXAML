namespace OmniXaml.Tests
{
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

            var clrAssemblyPair = new ClrAssemblyPair(type.Assembly, type.Namespace);
            nsRegistryMock.Setup(registry => registry.GetXamlNamespace("root"))
                .Returns(new XamlNamespace("root", new[] { clrAssemblyPair }));
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
