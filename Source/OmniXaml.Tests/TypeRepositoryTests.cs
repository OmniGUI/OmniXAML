namespace OmniXaml.Tests
{
    using Builder;
    using Classes;
    using Common;
    using Xunit;
    using Moq;
    using Typing;

    public class TypeRepositoryTests : GivenARuntimeTypeSource
    {
        private readonly Mock<INamespaceRegistry> nsRegistryMock;
        private TypeRepository sut;

        public TypeRepositoryTests()
        {
            nsRegistryMock = new Mock<INamespaceRegistry>();

            var type = typeof(DummyClass);

            var fullyConfiguredMapping = XamlNamespace
                .Map("root")
                .With(new[] {Route.Assembly(type.Assembly).WithNamespaces(new[] {type.Namespace})});

            nsRegistryMock.Setup(registry => registry.GetNamespace("root"))
                .Returns(fullyConfiguredMapping);

            nsRegistryMock.Setup(registry => registry.GetNamespace("clr-namespace:DummyNamespace;Assembly=DummyAssembly"))
                .Returns(new ClrNamespace(type.Assembly, type.Namespace));

            sut = new TypeRepository(nsRegistryMock.Object, new TypeFactoryDummy(), new TypeFeatureProviderDummy());
        }

        [Fact]
        public void GetWithFullAddressReturnsCorrectType()
        {          
            var xamlType = sut.GetByFullAddress(new XamlTypeName("root", "DummyClass"));

            Assert.Equal(xamlType.UnderlyingType, typeof(DummyClass));
        }

        [Fact]
        public void GetWithFullAddressOfClrNamespaceReturnsTheCorrectType()
        {
            var xamlType = sut.GetByFullAddress(new XamlTypeName("clr-namespace:DummyNamespace;Assembly=DummyAssembly", "DummyClass"));

            Assert.Equal(xamlType.UnderlyingType, typeof(DummyClass));
        }

        [Fact]
        public void GetByQualifiedName_ForTypeInDefaultNamespace()
        {
            sut = new TypeRepository(RuntimeTypeSource, new TypeFactoryDummy(), new TypeFeatureProviderDummy());

            var xamlType = sut.GetByQualifiedName("DummyClass");

            Assert.Equal(xamlType.UnderlyingType, typeof(DummyClass));
        }

        [Fact]
        public void FullAddressOfUnknownThrowNotFound()
        {
            const string unreachableTypeName = "UnreachableType";
            Assert.Throws<TypeNotFoundException>(() => sut.GetByFullAddress(new XamlTypeName("root", unreachableTypeName)));
       }     
    }
}

