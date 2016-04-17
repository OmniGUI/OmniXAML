namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Classes.WpfLikeModel;
    using Common;
    using ObjectAssembler;
    using Resources;
    using TypeConversion;
    using Xunit;

    public class ObjectAssemblerTests : GivenARuntimeTypeSource
    {
        public ObjectAssemblerTests()
        {
            source = new InstructionResources(this);
            sut = CreateSut();
        }

        private readonly InstructionResources source;
        private readonly IObjectAssembler sut;

        private ObjectAssembler CreateSut()
        {
            var topDownValueContext = new TopDownValueContext();
            var valueConnectionContext = new ValueContext(RuntimeTypeSource, topDownValueContext, new Dictionary<string, object>());
            return new ObjectAssembler(RuntimeTypeSource, valueConnectionContext, new Settings {InstanceLifeCycleListener = new TestListener()});
        }

        private IObjectAssembler CreateSutForLoadingSpecificInstance(object instance)
        {
            var topDownValueContext = new TopDownValueContext();
            var valueConnectionContext = new ValueContext(RuntimeTypeSource, topDownValueContext, new Dictionary<string, object>());

            var settings = new Settings {RootInstance = instance};

            var assembler = new ObjectAssembler(RuntimeTypeSource, valueConnectionContext, settings);
            return assembler;
        }

        [Fact]
        public void AttachableMemberThatIsCollection()
        {
            sut.Process(source.AttachableMemberThatIsCollection);
            var instance = sut.Result;
            var col = Container.GetCollection(instance);

            Assert.NotEmpty(col);
        }

        [Fact]
        public void AttemptToAssignItemsToNonCollectionMember()
        {
            Assert.Throws<ParseException>(() => sut.Process(source.AttemptToAssignItemsToNonCollectionMember));
        }

        [Fact]
        public void ChildIsAssociatedBeforeItsPropertiesAreSet()
        {
            sut.Process(source.InstanceWithChildAndProperty);
            var result = (DummyClass) sut.Result;

            Assert.False(result.TitleWasSetBeforeBeingAssociated);
        }

        [Fact]
        public void CollectionWithInnerCollection()
        {
            sut.Process(source.CollectionWithInnerCollection);

            var result = sut.Result;
            var children = ((DummyClass) result).Items;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(3, children.Count);
            Assert.All(children, child => Assert.IsType(typeof(Item), child));
            var innerCollection = children[0].Children;
            Assert.Equal(2, innerCollection.Count);
            Assert.All(innerCollection, child => Assert.IsType(typeof(Item), child));
        }

        [Fact]
        public void CorrectInstanceSetupSequence()
        {
            var expectedSequence = new[] {SetupSequence.Begin, SetupSequence.AfterAssociatedToParent, SetupSequence.AfterSetProperties, SetupSequence.End};
            sut.Process(source.InstanceWithChild);

            var listener = (TestListener) sut.LifecycleListener;
            Assert.Equal(expectedSequence.ToList().AsReadOnly(), listener.InvocationOrder);
        }

        [Fact]
        public void CustomCollection()
        {
            sut.Process(source.CustomCollection);
            Assert.NotEmpty((IEnumerable) sut.Result);
        }

        [Fact]
        public void DirectContentForOneToMany()
        {
            sut.Process(source.DirectContentForOneToMany);
        }

        [Fact]
        public void ExpandedAttachablePropertyAndItemBelow()
        {
            sut.Process(source.ExpandedAttachablePropertyAndItemBelow);

            var items = ((DummyClass) sut.Result).Items;

            var firstChild = items.First();
            var attachedProperty = Container.GetProperty(firstChild);
            Assert.Equal(2, items.Count);
            Assert.Equal("Value", attachedProperty);
        }

        [Fact]
        public void ExplicitCollection_ShouldHaveItems()
        {
            sut.Process(source.ExplicitCollection);
            var actual = (RootObject) sut.Result;

            var customCollection = actual.Collection;

            Assert.NotEmpty(customCollection);
        }

        [Fact]
        public void ExplicitCollection_ShouldReplaceCollectionInstance()
        {
            sut.Process(source.ExplicitCollection);
            var actual = (RootObject) sut.Result;

            Assert.True(actual.CollectionWasReplaced);
        }

        [Fact]
        public void ExtensionThatReturnsNull()
        {
            sut.Process(source.ExtensionThatReturnsNull);

            var result = sut.Result;
            var property = ((DummyClass) result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Null(property);
        }

        [Fact]
        public void ExtensionWithArgument()
        {
            sut.Process(source.ExtensionWithArgument);

            var result = sut.Result;
            var property = ((DummyClass) result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("Option", property);
        }

        [Fact]
        public void ExtensionWithNonStringArgument()
        {
            sut.Process(source.ExtensionWithNonStringArgument);

            var result = sut.Result;
            var property = ((DummyClass) result).Number;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(123, property);
        }

        [Fact]
        public void ExtensionWithTwoArguments()
        {
            sut.Process(source.ExtensionWithTwoArguments);

            var result = sut.Result;
            var property = ((DummyClass) result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("OneSecond", property);
        }

        [Fact]
        public void ImplicitCollection_ShouldHaveItems()
        {
            sut.Process(source.ImplicitCollection);
            var actual = (RootObject) sut.Result;

            var customCollection = actual.Collection;

            Assert.NotEmpty(customCollection);
        }

        [Fact]
        public void ImplicitCollection_ShouldKeepSameCollectionInstance()
        {
            sut.Process(source.ImplicitCollection);
            var actual = (RootObject) sut.Result;

            Assert.False(actual.CollectionWasReplaced);
        }

        [Fact]
        public void KeyDirective()
        {
            sut.Process(source.KeyDirective);

            var actual = sut.Result;
            Assert.IsType(typeof(DummyClass), actual);
            var dictionary = (IDictionary) ((DummyClass) actual).Resources;
            Assert.True(dictionary.Count > 0);
        }

        [Fact]
        public void ListBoxWithItemAndTextBlockNoNames()
        {
            sut.Process(source.ListBoxWithItemAndTextBlockNoNames);

            var w = (Window) sut.Result;
            var lb = (ListBox) w.Content;
            var lvi = (ListBoxItem) lb.Items.First();
            var tb = lvi.Content;

            Assert.IsType(typeof(TextBlock), tb);
        }

        [Fact]
        public void ListBoxWithItemAndTextBlockWithNames_HaveCorrectNames()
        {
            sut.Process(source.ListBoxWithItemAndTextBlockWithNames);

            var w = (Window) sut.Result;
            var lb = (ListBox) w.Content;
            var lvi = (ListBoxItem) lb.Items.First();
            var tb = (TextBlock) lvi.Content;

            Assert.Equal("MyListBox", lb.Name);
            Assert.Equal("MyListBoxItem", lvi.Name);
            Assert.Equal("MyTextBlock", tb.Name);
        }

        [Fact]
        public void MemberAfterInitalizationValue()
        {
            sut.Process(source.MemberAfterInitalizationValue);

            var root = (RootObject) sut.Result;
            var str = root.Collection[0];
            var dummy = (DummyClass) root.Collection[1];

            Assert.Equal("foo", str);
            Assert.Equal(123, dummy.Number);
        }

        [Fact]
        public void MemberWithIncompatibleTypes()
        {
            sut.Process(source.MemberWithIncompatibleTypes);

            var result = sut.Result;
            var property = ((DummyClass) result).Number;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(12, property);
        }

        [Fact]
        public void MixedCollection()
        {
            sut.Process(source.MixedCollection);
            var result = sut.Result;
            Assert.IsType(typeof(ArrayList), result);
            var arrayList = (ArrayList) result;
            Assert.True(arrayList.Count > 0);
        }

        [Fact]
        public void MixedCollectionWithRootInstance()
        {
            var root = new ArrayList();
            var assembler = CreateSutForLoadingSpecificInstance(root);
            assembler.Process(source.MixedCollection);
            var result = assembler.Result;
            Assert.IsType(typeof(ArrayList), result);
            var arrayList = (ArrayList) result;
            Assert.True(arrayList.Count > 0);
        }

        [Fact]
        public void NamedObject_HasCorrectName()
        {
            sut.Process(source.NamedObject);
            var result = sut.Result;
            var tb = (TextBlock) result;

            Assert.Equal("MyTextBlock", tb.Name);
        }

        [Fact]
        public void ObjectWithChild()
        {
            sut.Process(source.ObjectWithChild);

            var result = sut.Result;
            var property = ((DummyClass) result).Child;

            Assert.IsType(typeof(DummyClass), result);
            Assert.IsType(typeof(ChildClass), property);
        }

        [Fact]
        public void ObjectWithEnumMember()
        {
            sut.Process(source.ObjectWithEnumMember);

            var result = sut.Result;
            var property = ((DummyClass) result).EnumProperty;

            Assert.IsType<DummyClass>(result);
            Assert.Equal(SomeEnum.One, property);
        }

        [Fact]
        public void ObjectWithMember()
        {
            sut.Process(source.ObjectWithMember);

            var result = sut.Result;
            var property = ((DummyClass) result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("Property!", property);
        }

        [Fact]
        public void ObjectWithNullableEnumProperty()
        {
            sut.Process(source.ObjectWithNullableEnumProperty);

            var result = sut.Result;
            var property = ((DummyClass) result).EnumProperty;

            Assert.IsType<DummyClass>(result);
            Assert.Equal(SomeEnum.One, property);
        }

        [Fact]
        public void ObjectWithTwoMembers()
        {
            sut.Process(source.ObjectWithTwoMembers);

            var result = sut.Result;
            var property1 = ((DummyClass) result).SampleProperty;
            var property2 = ((DummyClass) result).AnotherProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("Property!", property1);
            Assert.Equal("Another!", property2);
        }

        [Fact]
        public void OneObject()
        {
            sut.Process(source.OneObject);

            var result = sut.Result;

            Assert.IsType(typeof(DummyClass), result);
        }

        [Fact]
        public void PropertyShouldBeAssignedBeforeChildIsAssociatedToItsParent()
        {
            sut.Process(source.ParentShouldReceiveInitializedChild);
            var parent = (SpyingParent) sut.Result;
            Assert.True(parent.ChildHadNamePriorToBeingAssigned);
        }

        [Fact]
        public void PureCollection()
        {
            sut.Process(source.PureCollection);
            var actual = (ArrayList) sut.Result;
            Assert.NotEmpty(actual);
        }

        [Fact]
        public void RootInstanceWithAttachableMember()
        {
            var root = new DummyClass();
            var sut = CreateSutForLoadingSpecificInstance(root);
            sut.Process(source.RootInstanceWithAttachableMember);
            var result = sut.Result;
            var attachedProperty = Container.GetProperty(result);
            Assert.Equal("Value", attachedProperty);
        }

        [Fact]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            sut.Process(source.GetString(sysNs));

            var actual = sut.Result;
            Assert.IsType(typeof(string), actual);
            Assert.Equal("Text", actual);
        }


        [Fact]
        public void TopDownContainsOuterObject()
        {
            sut.Process(source.InstanceWithChild);

            var dummyClassXamlType = RuntimeTypeSource.GetByType(typeof(DummyClass));
            var lastInstance = sut.TopDownValueContext.GetLastInstance(dummyClassXamlType);

            Assert.IsType(typeof(DummyClass), lastInstance);
        }

        [Fact]
        public void TwoChildrenWithNoRoot_ShouldThrow()
        {
            Assert.Throws<ParseException>(() => sut.Process(source.TwoRoots));
        }

        [Fact]
        public void TwoNestedNamedObjects_HaveCorrectNames()
        {
            sut.Process(source.TwoNestedNamedObjects);
            var result = sut.Result;
            var lbi = (ListBoxItem) result;
            var textBlock = (TextBlock) lbi.Content;

            Assert.Equal("MyListBoxItem", lbi.Name);
            Assert.Equal("MyTextBlock", textBlock.Name);
        }

        [Fact]
        public void WithCollection()
        {
            sut.Process(source.CollectionWithMoreThanOneItem);

            var result = sut.Result;
            var children = ((DummyClass) result).Items;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(3, children.Count);
            Assert.All(children, child => Assert.IsType(typeof(Item), child));
        }

        [Fact]
        public void WithCollectionAndInnerAttribute()
        {
            sut.Process(source.WithCollectionAndInnerAttribute);

            var result = sut.Result;
            var children = ((DummyClass) result).Items;
            var firstChild = children.First();
            var property = firstChild.Title;

            Assert.IsType(typeof(DummyClass), result);
            Assert.All(children, child => Assert.IsType(typeof(Item), child));
            Assert.Equal("SomeText", property);
        }
    }
}