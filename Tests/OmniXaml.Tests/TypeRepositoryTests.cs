namespace OmniXaml.Tests
{
    using Builder;
    using Classes;
    using Common.DotNetFx;
    using Xunit;
    using Moq;
    using Typing;

    public class TypeRepositoryTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
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

    public class TypeFeatureProviderTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {
        public ITypeFeatureProvider CreateSut()
        {
            return new TypeFeatureProvider(null);
        }

        [Fact]
        public void DependsOnRegister()
        {
            var sut = CreateSut();
            var expectedMetadata = new GenericMetadata<DummyClass>();
            expectedMetadata.WithMemberDependency(d => d.Items, d => d.AnotherProperty);
            TypeRepositoryMixin.RegisterMetadata(sut, expectedMetadata);

            var metadata = sut.GetMetadata<DummyClass>();
            Assert.Equal(expectedMetadata.AsNonGeneric(), metadata);
        }

        [Fact]
        public void GetMetadata()
        {
            var sut = CreateSut();
            var expected = new GenericMetadata<DummyClass>();

            sut.RegisterMetadata(expected);
            
            var actual = sut.GetMetadata<DummyClass>();

            Assert.Equal(expected.AsNonGeneric(), actual);
        }

        [Fact]
        public void GetMetadataOfSubClass_ReturnsPreviousParentMetadata()
        {
            var sut = CreateSut();
            var expected = new GenericMetadata<DummyObject>();

            sut.RegisterMetadata(expected);
            var actual = sut.GetMetadata<DummyClass>();

            Assert.Equal(expected.AsNonGeneric(), actual);
        }

        [Fact]
        public void GivenMetadataDefinitionsForBothClassAndSubclass_GetMetadataOfSubClass_ReturnsItsOwnMetadata()
        {
            var sut = CreateSut();
            var expected = new GenericMetadata<DummyClass>();

            sut.RegisterMetadata(expected);
            sut.RegisterMetadata(new GenericMetadata<DummyObject>());
            var actual = sut.GetMetadata<DummyClass>();

            Assert.Equal(expected.AsNonGeneric(), actual);
        }

        [Fact]
        public void GivenMetadataDefinitionsForParentAndGrandParent_GetMetadataOfChild_ReturnsParentMetadata()
        {
            var sut = CreateSut();
            var expected = new GenericMetadata<DummyClass>();

            sut.RegisterMetadata(new GenericMetadata<DummyObject>());
            sut.RegisterMetadata(expected);

            var actual = sut.GetMetadata<DummyChild>();

            Assert.Equal(expected.AsNonGeneric(), actual);
        }
    }
}

