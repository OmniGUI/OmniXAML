namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Classes.WpfLikeModel;
    using Common;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectAssembler;
    using Resources;
    using Xunit;
    using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    [TestClass]
    public class ObjectAssemblerTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private XamlInstructionResources source;
        private IObjectAssembler sut;
        private TopDownValueContext topDownValueContext;

        public ObjectAssemblerTests()
        {
            source = new XamlInstructionResources(this);
        }

        [TestInitialize]
        public void Initialize()
        {
            topDownValueContext = new TopDownValueContext();
            sut = new ObjectAssembler(WiringContext, topDownValueContext);
        }

        public IObjectAssembler CreateSut()
        {
            return new ObjectAssembler(WiringContext, new TopDownValueContext());
        }

        public IObjectAssembler CreateSutForLoadingSpecificInstance(object instance)
        {
            var settings = new ObjectAssemblerSettings { RootInstance = instance };
            var assembler = new ObjectAssembler(WiringContext, new TopDownValueContext(), settings);
            return assembler;
        }

        [TestMethod]
        public void OneObject()
        {
            sut.Process(source.OneObject);

            var result = sut.Result;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
        }

        [TestMethod]
        public void ObjectWithMember()
        {
            sut.Process(source.ObjectWithMember);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual("Property!", property);
        }

        [TestMethod]
        public void ObjectWithTwoMembers()
        {
            sut.Process(source.ObjectWithTwoMembers);

            var result = sut.Result;
            var property1 = ((DummyClass)result).SampleProperty;
            var property2 = ((DummyClass)result).AnotherProperty;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual("Property!", property1);
            Assert.AreEqual("Another!", property2);
        }

        [TestMethod]
        public void ObjectWithChild()
        {
            sut.Process(source.ObjectWithChild);

            var result = sut.Result;
            var property = ((DummyClass)result).Child;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.IsInstanceOfType(property, typeof(ChildClass));
        }

        [TestMethod]
        public void WithCollection()
        {
            sut.Process(source.CollectionWithMoreThanOneItem);

            var result = sut.Result;
            var children = ((DummyClass)result).Items;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual(3, children.Count);
            CollectionAssert.AllItemsAreInstancesOfType(children, typeof(Item));
        }

        [TestMethod]
        public void CollectionWithInnerCollection()
        {
            sut.Process(source.CollectionWithInnerCollection);

            var result = sut.Result;
            var children = ((DummyClass)result).Items;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual(3, children.Count);
            CollectionAssert.AllItemsAreInstancesOfType(children, typeof(Item));
            var innerCollection = children[0].Children;
            Assert.AreEqual(2, innerCollection.Count);
            CollectionAssert.AllItemsAreInstancesOfType(innerCollection, typeof(Item));
        }

        [TestMethod]
        public void WithCollectionAndInnerAttribute()
        {
            sut.Process(source.WithCollectionAndInnerAttribute);

            var result = sut.Result;
            var children = ((DummyClass)result).Items;
            var firstChild = children.First();
            var property = firstChild.Title;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            CollectionAssert.AllItemsAreInstancesOfType(children, typeof(Item));
            Assert.AreEqual("SomeText", property);
        }

        [TestMethod]
        public void MemberWithIncompatibleTypes()
        {
            sut.Process(source.MemberWithIncompatibleTypes);

            var result = sut.Result;
            var property = ((DummyClass)result).Number;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual(12, property);
        }

        [TestMethod]
        public void ExtensionWithArgument()
        {
            sut.Process(source.ExtensionWithArgument);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual("Option", property);
        }

        [TestMethod]
        public void ExtensionWithTwoArguments()
        {
            sut.Process(source.ExtensionWithTwoArguments);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual("OneSecond", property);
        }

        [TestMethod]
        public void ExtensionThatReturnsNull()
        {
            sut.Process(source.ExtensionThatReturnsNull);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.IsNull(property);
        }

        [TestMethod]
        public void ExtensionWithNonStringArgument()
        {
            sut.Process(source.ExtensionWithNonStringArgument);

            var result = sut.Result;
            var property = ((DummyClass)result).Number;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual(123, property);
        }

        [TestMethod]
        public void KeyDirective()
        {
            sut.Process(source.KeyDirective);

            var actual = sut.Result;
            Assert.IsInstanceOfType(actual, typeof(DummyClass));
            var dictionary = (IDictionary)((DummyClass)actual).Resources;
            Assert.IsTrue(dictionary.Count > 0);
        }

        [TestMethod]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            sut.Process(source.GetString(sysNs));

            var actual = sut.Result;
            Assert.IsInstanceOfType(actual, typeof(string));
            Assert.AreEqual("Text", actual);
        }


        [TestMethod]
        public void TopDownContainsOuterObject()
        {
            sut.Process(source.InstanceWithChild);

            var dummyClassXamlType = WiringContext.TypeContext.GetXamlType(typeof(DummyClass));
            var lastInstance = topDownValueContext.GetLastInstance(dummyClassXamlType);

            Assert.IsInstanceOfType(lastInstance, typeof(DummyClass));
        }

        [TestMethod]
        [ExpectedException(typeof(XamlParseException))]
        public void AttemptToAssignItemsToNonCollectionMember()
        {
            sut.Process(source.AttemptToAssignItemsToNonCollectionMember);
        }

        [TestMethod]
        [ExpectedException(typeof(XamlParseException))]
        public void TwoChildrenWithNoRoot_ShouldThrow()
        {
            sut.Process(source.TwoRoots);
        }

        [TestMethod]
        public void PropertyShouldBeAssignedBeforeChildIsAssociatedToItsParent()
        {
            sut.Process(source.ParentShouldReceiveInitializedChild);
            var parent = (SpyingParent)sut.Result;
            Assert.IsTrue(parent.ChildHadNamePriorToBeingAssigned);
        }

        [TestMethod]
        public void MixedCollection()
        {
            sut.Process(source.MixedCollection);
            var result = sut.Result;
            Assert.IsInstanceOfType(result, typeof(ArrayList));
            var arrayList = (ArrayList)result;
            Assert.IsTrue(arrayList.Count > 0);
        }

        [TestMethod]
        public void MixedCollectionWithRootInstance()
        {
            var root = new ArrayList();
            var assembler = CreateSutForLoadingSpecificInstance(root);
            assembler.Process(source.MixedCollection);
            var result = assembler.Result;
            Assert.IsInstanceOfType(result, typeof(ArrayList));
            var arrayList = (ArrayList)result;
            Assert.IsTrue(arrayList.Count > 0);
        }

        [Fact]
        public void RootInstanceWithAttachableMember()
        {
            var root = new DummyClass();
            var sut = CreateSutForLoadingSpecificInstance(root);
            sut.Process(source.RootInstanceWithAttachableMember);
            var result = sut.Result;
            var attachedProperty = Container.GetProperty(result);
            Xunit.Assert.Equal("Value", attachedProperty);
        }

        [TestMethod]
        public void NamedObject_HasCorrectName()
        {
            sut.Process(source.NamedObject);
            var result = sut.Result;
            var tb = (TextBlock)result;

            Assert.AreEqual("MyTextBlock", tb.Name);
        }

        [TestMethod]
        public void TwoNestedNamedObjects_HaveCorrectNames()
        {
            sut.Process(source.TwoNestedNamedObjects);
            var result = sut.Result;
            var lbi = (ListBoxItem)result;
            var textBlock = (TextBlock)lbi.Content;

            Assert.AreEqual("MyListBoxItem", lbi.Name);
            Assert.AreEqual("MyTextBlock", textBlock.Name);
        }

        [TestMethod]
        public void ListBoxWithItemAndTextBlockNoNames()
        {
            sut.Process(source.ListBoxWithItemAndTextBlockNoNames);

            var w = (Window)sut.Result;
            var lb = (ListBox)w.Content;
            var lvi = (ListBoxItem)lb.Items.First();
            var tb = lvi.Content;

            Assert.IsInstanceOfType(tb, typeof(TextBlock));
        }

        [TestMethod]
        public void ListBoxWithItemAndTextBlockWithNames_HaveCorrectNames()
        {
            sut.Process(source.ListBoxWithItemAndTextBlockWithNames);

            var w = (Window)sut.Result;
            var lb = (ListBox)w.Content;
            var lvi = (ListBoxItem)lb.Items.First();
            var tb = (TextBlock)lvi.Content;

            Assert.AreEqual("MyListBox", lb.Name);
            Assert.AreEqual("MyListBoxItem", lvi.Name);
            Assert.AreEqual("MyTextBlock", tb.Name);
        }

        [TestMethod]
        public void DirectContentForOneToMany()
        {
            sut.Process(source.DirectContentForOneToMany);
        }

        [TestMethod]
        public void CorrectInstanceSetupSequence()
        {
            var expectedSequence = new[] { SetupSequence.Begin, SetupSequence.AfterSetProperties, SetupSequence.AfterAssociatedToParent, SetupSequence.End };
            var actualSequence = new Collection<SetupSequence>();

            sut.InstanceLifeCycleHandler = new InstanceLifeCycleHandler
            {
                OnBegin = o => { if (o is ChildClass) actualSequence.Add(SetupSequence.Begin); },
                AfterProperties = o => { if (o is ChildClass) actualSequence.Add(SetupSequence.AfterSetProperties); },
                OnAssociatedToParent = o => { if (o is ChildClass)  actualSequence.Add(SetupSequence.AfterAssociatedToParent); },
                OnEnd = o => { if (o is ChildClass)  actualSequence.Add(SetupSequence.End); }
            };

            sut.Process(source.InstanceWithChild);

            CollectionAssert.AreEqual(expectedSequence, actualSequence);
        }
    }
}