namespace OmniXaml.AppServices.Tests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Builder;
    using ObjectAssembler;
    using OmniXaml.Tests.Classes;
    using Services;
    using TypeConversion;
    using Typing;
    using Xunit;

    public class ObjectAssemblerAdvancedNamescopeTests
    {
        [Fact]
        public void MultinameRegistrationGivesSameObjectTwoSubsequentNames()
        {
            var runtimeTypeContext = CreateRuntimeTypeContext();
            var x = CreateBuilder(runtimeTypeContext);

            var sut = new ObjectAssembler(runtimeTypeContext, new TopDownValueContext());

            var batch = new Collection<Instruction>
            {
                x.StartObject<DummyClass>(),
                x.StartMember<DummyClass>(d => d.Child),
                x.StartObject<ChildClass>(),
                x.StartMember<ChildClass>(c => c.Name),
                x.Value("OuterName"),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
            };

            sut.Process(batch);

            var result = (DummyClass)sut.Result;
            var child = (DummyObject)result.Child;
            Assert.Equal("InnerName", child.NamesHistory[0]);
            Assert.Equal("OuterName", child.NamesHistory[1]);
        }

        [Fact]
        public void GivenChildWithPreviousName_LatestNameIsRegisteredInParent()
        {
            var runtimeTypeContext = CreateRuntimeTypeContext();
            var x = CreateBuilder(runtimeTypeContext);

            var sut = new ObjectAssembler(runtimeTypeContext, new TopDownValueContext());

            var batch = new Collection<Instruction>
            {
                x.StartObject<DummyClass>(),
                x.StartMember<DummyClass>(d => d.Child),
                x.StartObject<ChildClass>(),
                x.StartMember<ChildClass>(c => c.Name),
                x.Value("OuterName"),
                x.EndMember(),
                x.EndObject(),
                x.EndMember(),
                x.EndObject(),
            };

            sut.Process(batch);

            var result = (DummyClass)sut.Result;

            var childClass = result.Find("OuterName");
            Assert.NotNull(childClass);           
        }

        private static XamlInstructionBuilder CreateBuilder(ITypeRepository typeRepository)
        {
            return new XamlInstructionBuilder(typeRepository);
        }

        private static IRuntimeTypeSource CreateRuntimeTypeContext()
        {
            var typeFactory = new MultiFactory(
                new List<TypeFactoryRegistration>
                {
                    new TypeFactoryRegistration(new TypeFactory(), type => type != typeof (ChildClass)),
                    new TypeFactoryRegistration(new TypeFactoryMock((type, args) => new ChildClass {Name = "InnerName"}), type => type == typeof (ChildClass)),
                });

            var typeFeatureProvider = new TypeFeatureProvider(new TypeConverterProvider());
            var xamlTypeRepository = new TypeRepository(new NamespaceRegistry(), typeFactory, typeFeatureProvider);
            var typeContext = new RuntimeTypeSource(xamlTypeRepository, new NamespaceRegistry());
            typeFeatureProvider.RegisterMetadata(typeof (DummyObject), new Metadata {RuntimePropertyName = "Name"});
            return typeContext;
        }
    }
}