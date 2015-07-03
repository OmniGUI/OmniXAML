namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using Classes;
    using Glass;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NewAssembler;

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
            var sut = new SuperObjectAssembler(state, WiringContext);

            sut.Process(builder.StartObject(type));

            var actualType = sut.StateCommuter.XamlType;

            var expectedType = WiringContext.GetType(type);
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
                    XamlMember = WiringContext.GetMember(WiringContext.GetType(dummyClass.GetType()), "Child"),                    
                });

            var childClass = new ChildClass();
            state.Push(
                new Level
                {
                    Instance = childClass,
                });

            var sut = new SuperObjectAssembler(state, WiringContext);

            sut.Process(builder.EndObject());

            Assert.AreEqual(1, state.Count);
            var expectedInstance = state.CurrentValue.Instance;

            Assert.AreSame(expectedInstance, dummyClass);
            Assert.AreSame(((DummyClass) expectedInstance).Child, childClass);
        }

        [TestMethod]
        public void EndMemberAssignsInstanceToParentCollection()
        {
            var state = new StackingLinkedList<Level>();

            var dummyClass = new DummyClass();
            state.Push(
                new Level
                {
                    Instance = dummyClass,
                    XamlMember = WiringContext.GetMember(WiringContext.GetType(dummyClass.GetType()), "Items"),
                    IsCollectionHolderObject = true,
                });

            var item = new Item();
            state.Push(
                new Level
                {
                    Instance = item,
                });

            var sut = new SuperObjectAssembler(state, WiringContext);

            sut.Process(builder.EndObject());

            Assert.AreEqual(1, state.Count);
            var expectedInstance = state.CurrentValue.Instance;

            Assert.AreSame(expectedInstance, dummyClass);
            ICollection expectedCollection = new Collection<Item> { item };
            CollectionAssert.AreEqual(((DummyClass)expectedInstance).Items, expectedCollection);
        }     
    }
}