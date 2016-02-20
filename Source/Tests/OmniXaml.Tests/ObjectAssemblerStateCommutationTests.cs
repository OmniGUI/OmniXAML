namespace OmniXaml.Tests
{
    using System.Collections.ObjectModel;
    using Classes;
    using Common.DotNetFx;
    using Glass;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectAssembler;
    using ObjectAssembler.Commands;
    using TypeConversion;

    [TestClass]
    public class ObjectAssemblerStateCommutationTests : GivenARuntimeTypeSourceWithNodeBuildersNetCore
    {  
        [TestMethod]
        public void StartObjectShouldSetTheXamlType()
        {
            var state = new StackingLinkedList<Level>();
            state.Push(new Level());

            var type = typeof(DummyClass);
            IRuntimeTypeSource typeSource = RuntimeTypeSource;
            ITopDownValueContext topDownValueContext = new TopDownValueContext();
            var sut = CreateSut(state, typeSource, topDownValueContext);

            sut.Process(X.StartObject(type));

            var actualType = sut.StateCommuter.Current.XamlType;

            var expectedType = RuntimeTypeSource.GetByType(type);
            Assert.AreEqual(expectedType, actualType);
        }

        private static ObjectAssembler CreateSut(StackingLinkedList<Level> state, IRuntimeTypeSource typeSource, ITopDownValueContext topDownValueContext)
        {
            
            var vc = new ValueContext(typeSource, topDownValueContext);
            return new ObjectAssembler(state, typeSource, new NullLifecycleListener(), vc);
        }

        [TestMethod]
        public void EndMemberAssignsInstanceToParentProperty()
        {
            var state = new StackingLinkedList<Level>();

            var dummyClass = new DummyClass();
            var xamlType = RuntimeTypeSource.GetByType(dummyClass.GetType());
            state.Push(
                new Level
                {
                    Instance = dummyClass,
                    Member = xamlType.GetMember("Child"),
                });

            var childClass = new ChildClass();
            state.Push(
                new Level
                {
                    XamlType = xamlType,
                    Instance = childClass,
                });

            var sut = CreateSut(state, RuntimeTypeSource, new TopDownValueContext());

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
            var xamlType = RuntimeTypeSource.GetByType(dummyClass.GetType());

            state.Push(
                new Level
                {
                    Instance = dummyClass,
                    XamlType = xamlType,
                });

            var xamlMember = xamlType.GetMember("Items");
       
            var sut = CreateSut(state, RuntimeTypeSource, new TopDownValueContext());

            sut.Process(X.StartMember<DummyClass>(d => d.Items));

            Assert.AreEqual(1, state.Count);
            Assert.AreEqual(state.CurrentValue.Member, xamlMember);
        }

        [TestMethod]
        public void GivenConfiguredXamlType_StartMemberInstantiatesItSetsTheMember()
        {
            var state = new StackingLinkedList<Level>();

            var type = typeof(DummyClass);
            state.Push(
                new Level
                {
                    XamlType = RuntimeTypeSource.GetByType(type)
                });

            var xamlMember = RuntimeTypeSource.GetByType(type).GetMember("Items");
          
            var sut = CreateSut(state, RuntimeTypeSource, new TopDownValueContext());

            sut.Process(X.StartMember<DummyClass>(d => d.Items));

            Assert.AreEqual(1, state.Count);
            Assert.IsInstanceOfType(state.CurrentValue.Instance, type);
            Assert.AreEqual(state.CurrentValue.Member, xamlMember);
        }

        [TestMethod]
        public void GivenConfigurationOfCollection_GetObjectConfiguresTheLevel()
        {
            var state = new StackingLinkedList<Level>();

            var type = typeof(DummyClass);
            var xamlMember = RuntimeTypeSource.GetByType(type).GetMember("Items");

            state.Push(
                new Level
                {
                    Instance = new DummyClass(),
                    Member = xamlMember,
                });

            var sut = CreateSut(state, RuntimeTypeSource, new TopDownValueContext());

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
                    Member = RuntimeTypeSource.GetByType(typeof(DummyClass)).GetMember("Items"),
                });

            state.Push(new Level());
         
            var sut = CreateSut(state, RuntimeTypeSource, new TopDownValueContext());

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
                    Member = RuntimeTypeSource.GetByType(typeof(DummyClass)).GetMember("Items"),
                    IsGetObject = true,
                });

            state.Push(new Level { XamlType = RuntimeTypeSource.GetByType(typeof(Item)) });
          
            var sut = CreateSut(state, RuntimeTypeSource, new TopDownValueContext());

            sut.Process(X.EndObject());
            sut.Process(X.EndObject());

            Assert.AreEqual(0, state.Count);
        }    

        [TestMethod]
        public void GivenCtorArguments_WritingEndObjectMakesResultTheProvidedValueOfTheMarkupExtension()
        {
            var state = new StackingLinkedList<Level>();

            var xamlType = RuntimeTypeSource.GetByType(typeof(DummyClass));
            state.Push(
                new Level
                {
                    XamlType = xamlType,
                    Instance = new DummyClass(),
                    Member = xamlType.GetMember("SampleProperty")
                });

            var constructionArguments = new Collection<ConstructionArgument> { new ConstructionArgument("Value", "Value") };
            state.Push(
                new Level
                {
                    XamlType = RuntimeTypeSource.GetByType(typeof(DummyExtension)),
                    CtorArguments = constructionArguments,
                });

            var sut = CreateSut(state, RuntimeTypeSource, new TopDownValueContext());
            sut.Process(X.EndObject());

            Assert.AreEqual("Value", sut.Result);
        }
    }
}