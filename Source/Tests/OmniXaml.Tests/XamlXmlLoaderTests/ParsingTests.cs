namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Linq;
    using Classes;
    using Classes.WpfLikeModel;
    using Xunit;
    using Xaml.Tests.Resources;

    public class ParsingTests : GivenAXmlLoader
    {
        private readonly Type expectedType = typeof (DummyClass);

        [Fact]
        public void EmptyStreamThrows()
        {
            Assert.Throws<LoadException>(() => Loader.FromString(Dummy.Empty));
        }

        [Fact]
        public void UnknownElementThrows()
        {
            Assert.Throws<LoadException>(() => Loader.FromString(Dummy.UnknownType));
        }

        [Fact]
        public void BadFormatThrowsXamlReaderException()
        {
            Assert.Throws<LoadException>(() => Loader.FromString(Dummy.BadFormat));
        }

        [Fact]
        public void NoPrefixMapsToNamespaceAndReturnsTheCorrectInstance()
        {
            var actual = Loader.FromString(Dummy.RootNamespace);
            Assert.IsType(expectedType, actual);
        }

        [Fact]
        public void SimpleXamlWithCollapsedTagsShouldReadLikeExplicitEndingTag()
        {
            var actual = Loader.FromString(Dummy.CollapsedTag);
            Assert.IsType(expectedType, actual);
        }

        [Fact]
        public void DifferentNamespacesShouldReturnCorrectInstances()
        {
            var actual = Loader.FromString(Dummy.DifferentNamespaces);
            Assert.IsType(expectedType, actual);
        }

        [Fact]
        public void ReadInstanceWithChild()
        {
            var actualInstance = Loader.FromString(Dummy.InstanceWithChild);

            Assert.IsType(typeof(DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Debug.Assert(dummyClass != null, "dummyClass != null");
            Assert.IsType(typeof (ChildClass), dummyClass.Child);
        }

        [Fact]
        public void ReadInstanceWithThreeLevelsOfNesting()
        {
            var root = Loader.FromString(Dummy.ThreeLevelsOfNesting);

            var dummy = root as DummyClass;
            Assert.IsType(typeof (DummyClass), root); // The retrieved instance should be of type DummyClass

            Debug.Assert(dummy != null, "dummy != null");
            var level2Instance = dummy.Child;
            Assert.NotNull(level2Instance);

            var level3Instance = level2Instance.Child;
            Assert.NotNull(level3Instance);
        }

        [Fact]
        public void KeyDirective()
        {
            var actual = Loader.FromString(Dummy.KeyDirective);
            Assert.IsType(typeof (DummyClass), actual);
            var dictionary = (IDictionary) ((DummyClass) actual).Resources;
            Assert.True(dictionary.Count > 0);
        }

        [Fact]
        public void String()
        {
            var actual = Loader.FromString(Dummy.String);
            Assert.IsType(typeof (string), actual);
            Assert.Equal("Text", actual);
        }

        [Fact]
        public void StringAsProperty()
        {
            var actual = Loader.FromString(Dummy.StringAsProperty);
            Assert.IsType(typeof (DummyClass), actual);
            Assert.Equal("Text", ((DummyClass) actual).SampleProperty);
        }

        [Fact]
        public void StringWithWhitespace()
        {
            var actual = Loader.FromString(Dummy.StringWithWhitespace);
            Assert.IsType(typeof (string), actual);
            Assert.Equal("Text", actual);
        }

        [Fact]
        public void Int()
        {
            var actual = Loader.FromString(Dummy.Int);
            Assert.IsType(typeof (int), actual);
            Assert.Equal(123, actual);
        }

        [Fact]
        public void StringProperty()
        {
            var actualInstance = Loader.FromString(Dummy.StringProperty);

            Assert.IsType(typeof (DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass);
            Assert.Equal("Property!", dummyClass.SampleProperty);
        }

        [Fact]
        public void ExpandedStringProperty()
        {
            var actualInstance = Loader.FromString(Dummy.InnerContent);

            Assert.IsType(typeof (DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass);
            Assert.Equal("Property!", dummyClass.SampleProperty);
        }

        [Fact]
        public void InnerContentIsContentProperty()
        {
            var actualInstance = Loader.FromString(Dummy.ContentPropertyInInnerContent);

            Assert.IsType(typeof (TextBlock), actualInstance); // $"The retrieved instance should be of type {typeof (TextBlock)}"
            var dummyClass = actualInstance as TextBlock;
            Assert.NotNull(dummyClass);
            Assert.Equal("Hi all!!", dummyClass.Text);
        }

        [Fact]
        public void NonStringProperty()
        {
            var actualInstance = Loader.FromString(Dummy.NonStringProperty);

            Assert.IsType(typeof (DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass); // dummyClass != null
            Assert.Equal(1234, dummyClass.Number);
        }

        [Fact]
        public void ChildCollection()
        {
            var actualInstance = Loader.FromString(Dummy.ChildCollection);

            Assert.IsType(typeof (DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass);
            Assert.NotNull(dummyClass.Items);
            Assert.Equal(3, dummyClass.Items.Count);
        }

        [Fact]
        public void AttachedProperty()
        {
            var actualInstance = Loader.FromString(Dummy.AttachedProperty);

            Assert.IsType(typeof (DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass);
            Assert.Equal(Container.GetProperty(dummyClass), "Value");
        }

        [Fact]
        public void Ignorable()
        {
            var actualInstance = Loader.FromString(Dummy.Ignorable);

            Assert.IsType(typeof (DummyClass), actualInstance); // The retrieved instance should be of type DummyClass
            var dummyClass = actualInstance as DummyClass;
            Assert.NotNull(dummyClass);
            Assert.Equal("Property!", dummyClass.SampleProperty);
        }

        [Fact]        
        public void DirectiveInSpecialNamespaceThatIsNotX()
        {
            var actual = Loader.FromString(Dummy.KeyDirectiveNotInX);
            Assert.IsType(typeof(DummyClass), actual);
            var dictionary = (IDictionary)((DummyClass)actual).Resources;
            Assert.True(dictionary.Count > 0);
        }

        [Fact]
        public void ExpandedAttachablePropertyAndItemBelow()
        {
            var loadedObject = Loader.FromString(Dummy.ExpandedAttachablePropertyAndItemBelow);
            
            var items = ((DummyClass)loadedObject).Items;

            var firstChild = items.First();
            var attachedProperty = Container.GetProperty(firstChild);
            Assert.Equal(2, items.Count);
            Assert.Equal("Value", attachedProperty);
        }

        [Fact]
        public void PureCollection()
        {
            var actualInstance = Loader.FromString(Dummy.PureCollection);
            Assert.NotEmpty((IEnumerable) actualInstance);
        }

        [Fact]
        public void AttachableMemberThatIsCollection()
        {
            var instance = Loader.FromString(Dummy.AttachableMemberThatIsCollection);
            var col = Container.GetCollection(instance);

            Assert.NotEmpty(col);
        }

        [Fact]
        public void ChildInDeeperNameScopeWithNamesInTwoLevels_HaveCorrectNames()
        {
            var actual = Loader.FromString(Dummy.ChildInDeeperNameScopeWithNamesInTwoLevels);

            var w = (Window)actual;
            var lb = (ListBox)w.Content;
            var lvi = (ListBoxItem)lb.Items.First();
            var tb = (TextBlock)lvi.Content;

            Assert.Equal("MyListBox", lb.Name);
            Assert.Equal("MyListBoxItem", lvi.Name);
            Assert.Equal("MyTextBlock", tb.Name);
        }
    }
}
