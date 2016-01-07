namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Linq;
    using Classes;
    using Classes.WpfLikeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xaml.Tests.Resources;
    using Xunit;
    using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    [TestClass]
    public class ParsingTests : GivenAXamlXmlLoader
    {
        private readonly Type expectedType = typeof (DummyClass);

        [TestMethod]
        [ExpectedException(typeof (LoadException))]
        public void EmptyStreamThrows()
        {
            Loader.FromString(Dummy.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof (LoadException))]
        public void UnknownElementThrows()
        {
            Loader.FromString(Dummy.UnknownType);
        }

        [TestMethod]
        [ExpectedException(typeof (LoadException))]
        public void BadFormatThrowsXamlReaderException()
        {
            Loader.FromString(Dummy.BadFormat);
        }

        [TestMethod]
        public void NoPrefixMapsToNamespaceAndReturnsTheCorrectInstance()
        {
            var actual = Loader.FromString(Dummy.RootNamespace);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void SimpleXamlWithCollapsedTagsShouldReadLikeExplicitEndingTag()
        {
            var actual = Loader.FromString(Dummy.CollapsedTag);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void DifferentNamespacesShouldReturnCorrectInstances()
        {
            var actual = Loader.FromString(Dummy.DifferentNamespaces);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void ReadInstanceWithChild()
        {
            var actualInstance = Loader.FromString(Dummy.InstanceWithChild);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Debug.Assert(dummyClass != null, "dummyClass != null");
            Assert.IsInstanceOfType(dummyClass.Child, typeof (ChildClass));
        }

        [TestMethod]
        public void ReadInstanceWithThreeLevelsOfNesting()
        {
            var root = Loader.FromString(Dummy.ThreeLevelsOfNesting);

            var dummy = root as DummyClass;
            Assert.IsInstanceOfType(root, typeof (DummyClass), "The retrieved instance should be of type DummyClass");

            Debug.Assert(dummy != null, "dummy != null");
            var level2Instance = dummy.Child;
            Assert.IsNotNull(level2Instance);

            var level3Instance = level2Instance.Child;
            Assert.IsNotNull(level3Instance);
        }

        [TestMethod]
        public void KeyDirective()
        {
            var actual = Loader.FromString(Dummy.KeyDirective);
            Assert.IsInstanceOfType(actual, typeof (DummyClass));
            var dictionary = (IDictionary) ((DummyClass) actual).Resources;
            Assert.IsTrue(dictionary.Count > 0);
        }

        [TestMethod]
        public void String()
        {
            var actual = Loader.FromString(Dummy.String);
            Assert.IsInstanceOfType(actual, typeof (string));
            Assert.AreEqual("Text", actual);
        }

        [TestMethod]
        public void StringAsProperty()
        {
            var actual = Loader.FromString(Dummy.StringAsProperty);
            Assert.IsInstanceOfType(actual, typeof (DummyClass));
            Assert.AreEqual("Text", ((DummyClass) actual).SampleProperty);
        }

        [TestMethod]
        public void StringWithWhitespace()
        {
            var actual = Loader.FromString(Dummy.StringWithWhitespace);
            Assert.IsInstanceOfType(actual, typeof (string));
            Assert.AreEqual("Text", actual);
        }

        [TestMethod]
        public void Int()
        {
            var actual = Loader.FromString(Dummy.Int);
            Assert.IsInstanceOfType(actual, typeof (int));
            Assert.AreEqual(123, actual);
        }

        [TestMethod]
        public void StringProperty()
        {
            var actualInstance = Loader.FromString(Dummy.StringProperty);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Property!", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void ExpandedStringProperty()
        {
            var actualInstance = Loader.FromString(Dummy.InnerContent);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Property!", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void InnerContentIsContentProperty()
        {
            var actualInstance = Loader.FromString(Dummy.ContentPropertyInInnerContent);

            Assert.IsInstanceOfType(actualInstance, typeof (TextBlock), $"The retrieved instance should be of type {typeof (TextBlock)}");
            var dummyClass = actualInstance as TextBlock;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Hi all!!", dummyClass.Text);
        }

        [TestMethod]
        public void NonStringProperty()
        {
            var actualInstance = Loader.FromString(Dummy.NonStringProperty);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass, "dummyClass != null");
            Assert.AreEqual(1234, dummyClass.Number);
        }

        [TestMethod]
        public void ChildCollection()
        {
            var actualInstance = Loader.FromString(Dummy.ChildCollection);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.IsNotNull(dummyClass.Items);
            Assert.AreEqual(3, dummyClass.Items.Count);
        }

        [TestMethod]
        public void AttachedProperty()
        {
            var actualInstance = Loader.FromString(Dummy.AttachedProperty);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual(Container.GetProperty(dummyClass), "Value");
        }

        [TestMethod]
        public void Ignorable()
        {
            var actualInstance = Loader.FromString(Dummy.Ignorable);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Property!", dummyClass.SampleProperty);
        }

        [TestMethod]        
        public void DirectiveInSpecialNamespaceThatIsNotX()
        {
            var actual = Loader.FromString(Dummy.KeyDirectiveNotInX);
            Assert.IsInstanceOfType(actual, typeof(DummyClass));
            var dictionary = (IDictionary)((DummyClass)actual).Resources;
            Assert.IsTrue(dictionary.Count > 0);
        }

        [TestMethod]
        public void ExpandedAttachablePropertyAndItemBelow()
        {
            var loadedObject = Loader.FromString(Dummy.ExpandedAttachablePropertyAndItemBelow);
            
            var items = ((DummyClass)loadedObject).Items;

            var firstChild = items.First();
            var attachedProperty = Container.GetProperty(firstChild);
            Xunit.Assert.Equal(2, items.Count);
            Xunit.Assert.Equal("Value", attachedProperty);
        }

        [Fact]
        public void PureCollection()
        {
            var actualInstance = Loader.FromString(Dummy.PureCollection);
            Xunit.Assert.NotEmpty((IEnumerable) actualInstance);
        }

        [Fact]
        public void AttachableMemberThatIsCollection()
        {
            var instance = Loader.FromString(Dummy.AttachableMemberThatIsCollection);
            var col = Container.GetCollection(instance);

            Xunit.Assert.NotEmpty(col);
        }

        [TestMethod]
        public void ChildInDeeperNameScopeWithNamesInTwoLevels_HaveCorrectNames()
        {
            var actual = Loader.FromString(Dummy.ChildInDeeperNameScopeWithNamesInTwoLevels);

            var w = (Window)actual;
            var lb = (ListBox)w.Content;
            var lvi = (ListBoxItem)lb.Items.First();
            var tb = (TextBlock)lvi.Content;

            Assert.AreEqual("MyListBox", lb.Name);
            Assert.AreEqual("MyListBoxItem", lvi.Name);
            Assert.AreEqual("MyTextBlock", tb.Name);
        }
    }
}
