namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using Builder;
    using Classes;
    using Glass;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NewAssembler;
    using Typing;

    [TestClass]
    public class NewObjectAssemblerStateCommutationTests : GivenAWiringContext
    {
        private readonly XamlNodeBuilder builder;

        public NewObjectAssemblerStateCommutationTests()
        {
            builder = new XamlNodeBuilder(WiringContext.TypeContext);
        }

        [TestMethod]
        public void StartObjectShouldSetTheXamlType()
        {
            var state = new StackingLinkedList<Level>();
            state.Push(new Level());

            var type = typeof(DummyClass);
            var sut = new SuperObjectAssembler(state, WiringContext, new TopDownMemberValueContext());

            sut.Process(builder.StartObject(type));

            var actualType = sut.StateCommuter.XamlType;

            var expectedType = WiringContext.TypeContext.GetXamlType(type);
            Assert.AreEqual(expectedType, actualType);
        }

        [TestMethod]
        public void EndMemberAssignsInstanceToParentProperty()
        {
            var state = new StackingLinkedList<Level>();

            var dummyClass = new DummyClass();
            state.Push(
                new Level
                {
                    Instance = dummyClass,
                    XamlMember = WiringContext.TypeContext.GetXamlType(dummyClass.GetType()).GetMember("Child"),
                });

            var childClass = new ChildClass();
            state.Push(
                new Level
                {
                    Instance = childClass,
                });

            var sut = new SuperObjectAssembler(state, WiringContext, new TopDownMemberValueContext());

            sut.Process(builder.EndObject());

            Assert.AreEqual(1, state.Count);
            var expectedInstance = state.CurrentValue.Instance;

            Assert.AreSame(expectedInstance, dummyClass);
            Assert.AreSame(((DummyClass)expectedInstance).Child, childClass);
        }

        [TestMethod]
        public void GivenConfiguredInstance_StartMemberSetsTheMember()
        {
            var state = new StackingLinkedList<Level>();

            var dummyClass = new DummyClass();
            state.Push(
                new Level
                {
                    Instance = dummyClass,
                });

            var xamlMember = WiringContext.TypeContext.GetXamlType(dummyClass.GetType()).GetMember("Items");

            var sut = new SuperObjectAssembler(state, WiringContext, new TopDownMemberValueContext());

            sut.Process(builder.StartMember<DummyClass>(d => d.Items));

            Assert.AreEqual(1, state.Count);
            Assert.AreEqual(state.CurrentValue.XamlMember, xamlMember);
        }

        [TestMethod]
        public void GivenConfiguredXamlType_StartMemberInstantiatesItSetsTheMember()
        {
            var state = new StackingLinkedList<Level>();

            var type = typeof(DummyClass);
            state.Push(
                new Level
                {
                    XamlType = WiringContext.TypeContext.GetXamlType(type)
                });

            var xamlMember = WiringContext.TypeContext.GetXamlType(type).GetMember("Items");

            var sut = new SuperObjectAssembler(state, WiringContext, new TopDownMemberValueContext());

            sut.Process(builder.StartMember<DummyClass>(d => d.Items));

            Assert.AreEqual(1, state.Count);
            Assert.IsInstanceOfType(state.CurrentValue.Instance, type);
            Assert.AreEqual(state.CurrentValue.XamlMember, xamlMember);
        }

        [TestMethod]
        public void GivenConfigurationOfCollection_GetObjectConfiguresTheLevel()
        {
            var state = new StackingLinkedList<Level>();

            var type = typeof(DummyClass);
            var xamlMember = WiringContext.TypeContext.GetXamlType(type).GetMember("Items");

            state.Push(
                new Level
                {
                    Instance = new DummyClass(),
                    XamlMember = xamlMember,
                });

            var sut = new SuperObjectAssembler(state, WiringContext, new TopDownMemberValueContext());

            sut.Process(builder.GetObject());

            Assert.AreEqual(2, state.Count);
            Assert.IsTrue(state.CurrentValue.Collection != null);
            Assert.IsTrue(state.CurrentValue.IsGetObject);
        }

        [TestMethod]
        public void GivenStateAfterGetObject_WritingObjectAssignsItToTheCollection()
        {
            var state = new StackingLinkedList<Level>();
            var collection = new Collection<Item>();

            state.Push(
                new Level
                {
                    Collection = collection,
                    IsGetObject = true,
                });

            state.Push(new Level());

            var sut = new SuperObjectAssembler(state, WiringContext, new TopDownMemberValueContext());

            sut.Process(builder.StartObject<Item>());
            sut.Process(builder.EndObject());

            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void GivenStateAfterGetObject_NeedsTwoEndObjectsToConsumeAllTheLevels()
        {
            var state = new StackingLinkedList<Level>();
            var collection = new Collection<Item>();

            state.Push(
                new Level
                {
                    Collection = collection,
                    IsGetObject = true,
                });

            state.Push(new Level { XamlType = WiringContext.TypeContext.GetXamlType(typeof(Item)) });

            var sut = new SuperObjectAssembler(state, WiringContext, new TopDownMemberValueContext());

            sut.Process(builder.EndObject());
            sut.Process(builder.EndObject());

            Assert.AreEqual(0, state.Count);
        }    

        [TestMethod]
        public void GivenCtorArguments_WritingEndObjectMakesResultTheProvidedValueOfTheMarkupExtension()
        {
            var state = new StackingLinkedList<Level>();

            var xamlType = WiringContext.TypeContext.GetXamlType(typeof(DummyClass));
            state.Push(
                new Level
                {
                    XamlType = xamlType,
                    Instance = new DummyClass(),
                    XamlMember = xamlType.GetMember("SampleProperty")
                });

            var constructionArguments = new Collection<ConstructionArgument> { new ConstructionArgument("Value", "Value") };
            state.Push(
                new Level
                {
                    XamlType = WiringContext.TypeContext.GetXamlType(typeof(DummyExtension)),
                    CtorArguments = constructionArguments,
                });

            var sut = new SuperObjectAssembler(state, WiringContext, new TopDownMemberValueContext());
            sut.Process(builder.EndObject());

            Assert.AreEqual("Value", sut.Result);
        }
    }
}