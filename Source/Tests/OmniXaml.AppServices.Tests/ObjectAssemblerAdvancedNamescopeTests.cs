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
            var wiringContext = CreateWiringContext();
            var x = CreateBuilder(wiringContext);

            var sut = new ObjectAssembler(wiringContext, new TopDownValueContext());

            var batch = new Collection<XamlInstruction>
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
            var wiringContext = CreateWiringContext();
            var x = CreateBuilder(wiringContext);

            var sut = new ObjectAssembler(wiringContext, new TopDownValueContext());

            var batch = new Collection<XamlInstruction>
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

        private static XamlInstructionBuilder CreateBuilder(IXamlTypeRepository typeRepository)
        {
            return new XamlInstructionBuilder(typeRepository);
        }

        private static ITypeContext CreateWiringContext()
        {
            var typeFactory = new MultiFactory(
                new List<TypeFactoryRegistration>
                {
                    new TypeFactoryRegistration(new TypeFactory(), type => type != typeof (ChildClass)),
                    new TypeFactoryRegistration(new TypeFactoryMock((type, args) => new ChildClass {Name = "InnerName"}), type => type == typeof (ChildClass)),
                });

            var typeFeatureProvider = new TypeFeatureProvider(new ContentPropertyProvider(), new TypeConverterProvider());
            var xamlTypeRepository = new XamlTypeRepository(new XamlNamespaceRegistry(), typeFactory, typeFeatureProvider);
            var typeContext = new TypeContext(xamlTypeRepository, new XamlNamespaceRegistry());
            typeContext.RegisterMetadata(typeof (DummyObject), new Metadata {RuntimePropertyName = "Name"});
            return typeContext;
        }
    }
}