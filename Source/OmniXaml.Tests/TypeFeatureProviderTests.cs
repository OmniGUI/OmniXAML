namespace OmniXaml.Tests
{
    using Classes;
    using Common;
    using Typing;
    using Xunit;

    public class TypeFeatureProviderTests : GivenARuntimeTypeSource
    {
        private static ITypeFeatureProvider CreateSut()
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