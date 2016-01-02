namespace OmniXaml.Tests
{
    using Builder;
    using Classes;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Typing;

    [TestClass]
    public class XamlTypeRepositoryTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private readonly Mock<IXamlNamespaceRegistry> nsRegistryMock;
        private XamlTypeRepository sut;

        public XamlTypeRepositoryTests()
        {
            nsRegistryMock = new Mock<IXamlNamespaceRegistry>();

            var type = typeof(DummyClass);

            var fullyConfiguredMapping = XamlNamespace
                .Map("root")
                .With(new[] {Route.Assembly(type.Assembly).WithNamespaces(new[] {type.Namespace})});

            nsRegistryMock.Setup(registry => registry.GetNamespace("root"))
                .Returns(fullyConfiguredMapping);

            nsRegistryMock.Setup(registry => registry.GetNamespace("clr-namespace:DummyNamespace;Assembly=DummyAssembly"))
                .Returns(new ClrNamespace(type.Assembly, type.Namespace));
        }

        [TestInitialize]
        public void Initialize()
        {
            sut = new XamlTypeRepository(nsRegistryMock.Object, new TypeFactoryDummy(), new TypeFeatureProviderDummy());
        }

        [TestMethod]
        public void GetWithFullAddressReturnsCorrectType()
        {          
            var xamlType = sut.GetWithFullAddress(new XamlTypeName("root", "DummyClass"));

            Assert.AreEqual(xamlType.UnderlyingType, typeof(DummyClass));
        }

        [TestMethod]
        public void GetWithFullAddressOfClrNamespaceReturnsTheCorrectType()
        {
            var xamlType = sut.GetWithFullAddress(new XamlTypeName("clr-namespace:DummyNamespace;Assembly=DummyAssembly", "DummyClass"));

            Assert.AreEqual(xamlType.UnderlyingType, typeof(DummyClass));
        }

        [TestMethod]
        public void GetByQualifiedName_ForTypeInDefaultNamespace()
        {
            sut = new XamlTypeRepository(TypeContext, new TypeFactoryDummy(), new TypeFeatureProviderDummy());

            var xamlType = sut.GetByQualifiedName("DummyClass");

            Assert.AreEqual(xamlType.UnderlyingType, typeof(DummyClass));
        }

        [TestMethod]
        [ExpectedException(typeof(TypeNotFoundException))]
        public void FullAddressOfUnknownThrowNotFound()
        {
            const string unreachableTypeName = "UnreachableType";
            sut.GetWithFullAddress(new XamlTypeName("root", unreachableTypeName));
       }

        [TestMethod]
        public void DependsOnRegister()
        {
            var expectedMetadata = new GenericMetadata<DummyClass>();
            expectedMetadata.WithMemberDependency(d => d.Items, d => d.AnotherProperty);
            XamlTypeRepositoryMixin.RegisterMetadata(sut, expectedMetadata);

            var metadata = sut.GetMetadata<DummyClass>();
            Assert.AreEqual(expectedMetadata.AsNonGeneric(), metadata);
        }

        [TestMethod]
        public void GetMetadata()
        {
            var expected = new GenericMetadata<DummyClass>();

            sut.RegisterMetadata(expected);
            var actual = sut.GetMetadata<DummyClass>();

            Assert.AreEqual(expected.AsNonGeneric(), actual);
        }

        [TestMethod]
        public void GetMetadataOfSubClass_ReturnsPreviousParentMetadata()
        {
            var expected = new GenericMetadata<DummyObject>();

            sut.RegisterMetadata(expected);
            var actual = sut.GetMetadata<DummyClass>();

            Assert.AreEqual(expected.AsNonGeneric(), actual);
        }

        [TestMethod]
        public void GivenMetadataDefinitionsForBothClassAndSubclass_GetMetadataOfSubClass_ReturnsItsOwnMetadata()
        {
            var expected = new GenericMetadata<DummyClass>();

            sut.RegisterMetadata(expected);
            sut.RegisterMetadata(new GenericMetadata<DummyObject>());
            var actual = sut.GetMetadata<DummyClass>();

            Assert.AreEqual(expected.AsNonGeneric(), actual);
        }

        [TestMethod]
        public void GivenMetadataDefinitionsForParentAndGrandParent_GetMetadataOfChild_ReturnsParentMetadata()
        {
            var expected = new GenericMetadata<DummyClass>();

            sut.RegisterMetadata(new GenericMetadata<DummyObject>());
            sut.RegisterMetadata(expected);
            
            var actual = sut.GetMetadata<DummyChild>();

            Assert.AreEqual(expected.AsNonGeneric(), actual);
        }
    }
}

