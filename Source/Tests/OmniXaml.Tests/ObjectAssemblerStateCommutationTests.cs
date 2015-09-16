namespace OmniXaml.Tests
{
    using System.Collections.ObjectModel;
    using Classes;
    using Common;
    using Common.NetCore;
    using Glass;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectAssembler;
    using Typing;

    [TestClass]
    public class ObjectAssemblerStateCommutationTests : GivenAWiringContextWithNodeBuildersNetCore
    {  
        [TestMethod]
        public void StartObjectShouldSetTheXamlType()
        {
            var state = new StackingLinkedList<Level>();
            state.Push(new Level());

            var type = typeof(DummyClass);
            var sut = new ObjectAssembler(state, WiringContext, new TopDownValueContext());

            sut.Process(X.StartObject(type));

            var actualType = sut.StateCommuter.Current.XamlType;

            var expectedType = WiringContext.TypeContext.GetXamlType(type);
            Assert.AreEqual(expectedType, actualType);
        }

        [TestMethod]
        public void EndMemberAssignsInstanceToParentProperty()
        {
            var state = new StackingLinkedList<Level>();

            var dummyClass = new DummyClass();
            var xamlType = WiringContext.TypeContext.GetXamlType(dummyClass.GetType());
            state.Push(
                new Level
                {
                    Instance = dummyClass,
                    XamlMember = xamlType.GetMember("Child"),
                });

            var childClass = new ChildClass();
            state.Push(
                new Level
                {
                    XamlType = xamlType,
                    Instance = childClass,
                });

            var sut = new ObjectAssembler(state, WiringContext, new TopDownValueContext());

            sut.Process(X.EndObject());

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
            var xamlType = WiringContext.TypeContext.GetXamlType(dummyClass.GetType());

            state.Push(
                new Level
                {
                    Instance = dummyClass,
                    XamlType = xamlType,
                });

            var xamlMember = xamlType.GetMember("Items");

            var sut = new ObjectAssembler(state, WiringContext, new TopDownValueContext());

            sut.Process(X.StartMember<DummyClass>(d => d.Items));

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

            var sut = new ObjectAssembler(state, WiringContext, new TopDownValueContext());

            sut.Process(X.StartMember<DummyClass>(d => d.Items));

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

            var sut = new ObjectAssembler(state, WiringContext, new TopDownValueContext());

            sut.Process(X.GetObject());

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

            var sut = new ObjectAssembler(state, WiringContext, new TopDownValueContext());

            sut.Process(X.StartObject<Item>());
            sut.Process(X.EndObject());

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

            var sut = new ObjectAssembler(state, WiringContext, new TopDownValueContext());

            sut.Process(X.EndObject());
            sut.Process(X.EndObject());

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

            var sut = new ObjectAssembler(state, WiringContext, new TopDownValueContext());
            sut.Process(X.EndObject());

            Assert.AreEqual("Value", sut.Result);
        }
    }
}