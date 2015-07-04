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
        public void GivenConfiguredInstance_StartMemberSetsTheMemberAndRaisesLevel()
        {
            var state = new StackingLinkedList<Level>();

            var dummyClass = new DummyClass();
            state.Push(
                new Level
                {
                    Instance = dummyClass,                    
                });

            var xamlMember = WiringContext.GetMember(WiringContext.GetType(dummyClass.GetType()), "Items");

            var sut = new SuperObjectAssembler(state, WiringContext);

            sut.Process(builder.StartMember<DummyClass>(d => d.Items));

            Assert.AreEqual(2, state.Count);
            Assert.AreEqual(state.CurrentValue, new Level());
            Assert.AreEqual(state.PreviousValue.XamlMember, xamlMember);
        }

        [TestMethod]
        public void GivenConfiguredXamlType_StartMemberInstantiatesItSetsTheMemberAndRaisesLevel()
        {
            var state = new StackingLinkedList<Level>();

            var type = typeof (DummyClass);
            state.Push(
                new Level
                {
                    XamlType = WiringContext.GetType(type)
                });

            var xamlMember = WiringContext.GetMember(WiringContext.GetType(type), "Items");

            var sut = new SuperObjectAssembler(state, WiringContext);

            sut.Process(builder.StartMember<DummyClass>(d => d.Items));

            Assert.AreEqual(2, state.Count);
            Assert.AreEqual(state.CurrentValue, new Level());
            Assert.IsInstanceOfType(state.PreviousValue.Instance, type);
            Assert.AreEqual(state.PreviousValue.XamlMember, xamlMember);
        }

        [TestMethod]
        public void GivenConfigurationOfCollection_GetObjectConfiguresTheLevel()
        {
            var state = new StackingLinkedList<Level>();

            var type = typeof (DummyClass);
            var xamlMember = WiringContext.GetMember(WiringContext.GetType(type), "Items");

            state.Push(
                new Level
                {
                    Instance = new DummyClass(),
                    XamlMember = xamlMember,                    
                });

            state.Push(new Level());
            
            var sut = new SuperObjectAssembler(state, WiringContext);

            sut.Process(builder.GetObject());

            Assert.AreEqual(3, state.Count);
            Assert.IsTrue(state.PreviousValue.Collection != null);            
            Assert.IsTrue(state.PreviousValue.IsGetObject);
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

            var sut = new SuperObjectAssembler(state, WiringContext);

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

            state.Push(new Level {XamlType = WiringContext.GetType(typeof (Item))});

            var sut = new SuperObjectAssembler(state, WiringContext);

            sut.Process(builder.EndObject());
            sut.Process(builder.EndObject());

            Assert.AreEqual(0, state.Count);
        }
    }
}