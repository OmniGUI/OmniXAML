namespace OmniXaml.Tests
{
    using Classes;
    using Typing;
    using Xunit;

    public class MetadataProviderTests
    {
        [Fact]
        public void RegisterForType()
        {
            var medataData = new GenericMetadata<DummyObject>()
                .WithMemberDependency(o => o.Name, o2 => o2.Name)
                .WithRuntimeNameProperty(o => o.Name);

            var sut = new MetadataProvider();
            sut.Register(typeof(DummyObject), medataData);
            var actual = sut.Get(typeof(DummyObject));
            Assert.Equal(medataData.AsNonGeneric(), actual);
        }

        [Fact]
        public void RegisterForBaseAndDerived()
        {
            var metadataForBase = new GenericMetadata<DummyObject>()
                .WithRuntimeNameProperty(o => o.Name);

            var metadataForDerived = new GenericMetadata<DummyClass>()
                .WithMemberDependency(d => d.Item, d => d.Child);

            var sut = new MetadataProvider();
            sut.Register(typeof(DummyObject), metadataForBase);
            sut.Register(typeof(DummyClass), metadataForDerived);

            var expected = new Metadata
            {
                RuntimePropertyName = "Name",
                PropertyDependencies = new DependencyRegistrations { new DependencyRegistration("Item", "Child") }
            };


            var actual = sut.Get(typeof(DummyClass));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoRegistrationReturnsEmptyMetadata()
        {
            var sut = new MetadataProvider();

            var metadata = sut.Get(typeof(DummyObject));

            Assert.True(Metadata.IsEmpty(metadata));
        }
    }
}