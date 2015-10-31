namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Xml;
    using Classes;
    using Classes.WpfLikeModel;
    using Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xaml.Tests.Resources;

    [TestClass]
    public class ParsingTests : GivenAXamlXmlLoader
    {
        private readonly Type expectedType = typeof (DummyClass);

        [TestMethod]
        [ExpectedException(typeof (XamlLoadException))]
        public void EmptyStreamThrows()
        {
            XamlLoader.Load(Dummy.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof (XamlLoadException))]
        public void UnknownElementThrows()
        {
            XamlLoader.Load(Dummy.UnknownType);
        }

        [TestMethod]
        [ExpectedException(typeof (XamlLoadException))]
        public void BadFormatThrowsXamlReaderException()
        {
            XamlLoader.Load(Dummy.BadFormat);
        }

        [TestMethod]
        public void NoPrefixMapsToNamespaceAndReturnsTheCorrectInstance()
        {
            var actual = XamlLoader.Load(Dummy.RootNamespace);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void SimpleXamlWithCollapsedTagsShouldReadLikeExplicitEndingTag()
        {
            var actual = XamlLoader.Load(Dummy.CollapsedTag);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void DifferentNamespacesShouldReturnCorrectInstances()
        {
            var actual = XamlLoader.Load(Dummy.DifferentNamespaces);
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void ReadInstanceWithChild()
        {
            var actualInstance = XamlLoader.Load(Dummy.InstanceWithChild);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Debug.Assert(dummyClass != null, "dummyClass != null");
            Assert.IsInstanceOfType(dummyClass.Child, typeof (ChildClass));
        }

        [TestMethod]
        public void ReadInstanceWithThreeLevelsOfNesting()
        {
            var root = XamlLoader.Load(Dummy.ThreeLevelsOfNesting);

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
            var actual = XamlLoader.Load(Dummy.KeyDirective);
            Assert.IsInstanceOfType(actual, typeof (DummyClass));
            var dictionary = (IDictionary) ((DummyClass) actual).Resources;
            Assert.IsTrue(dictionary.Count > 0);
        }

        [TestMethod]
        public void String()
        {
            var actual = XamlLoader.Load(Dummy.String);
            Assert.IsInstanceOfType(actual, typeof (string));
            Assert.AreEqual("Text", actual);
        }

        [TestMethod]
        public void StringAsProperty()
        {
            var actual = XamlLoader.Load(Dummy.StringAsProperty);
            Assert.IsInstanceOfType(actual, typeof (DummyClass));
            Assert.AreEqual("Text", ((DummyClass) actual).SampleProperty);
        }

        [TestMethod]
        public void StringWithWhitespace()
        {
            var actual = XamlLoader.Load(Dummy.StringWithWhitespace);
            Assert.IsInstanceOfType(actual, typeof (string));
            Assert.AreEqual("Text", actual);
        }

        [TestMethod]
        public void Int()
        {
            var actual = XamlLoader.Load(Dummy.Int);
            Assert.IsInstanceOfType(actual, typeof (int));
            Assert.AreEqual(123, actual);
        }

        [TestMethod]
        public void StringProperty()
        {
            var actualInstance = XamlLoader.Load(Dummy.StringProperty);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Property!", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void ExpandedStringProperty()
        {
            var actualInstance = XamlLoader.Load(Dummy.InnerContent);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Property!", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void InnerContentIsContentProperty()
        {
            var actualInstance = XamlLoader.Load(Dummy.ContentPropertyInInnerContent);

            Assert.IsInstanceOfType(actualInstance, typeof (TextBlock), $"The retrieved instance should be of type {typeof (TextBlock)}");
            var dummyClass = actualInstance as TextBlock;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Hi all!!", dummyClass.Text);
        }

        [TestMethod]
        public void NonStringProperty()
        {
            var actualInstance = XamlLoader.Load(Dummy.NonStringProperty);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass, "dummyClass != null");
            Assert.AreEqual(1234, dummyClass.Number);
        }

        [TestMethod]
        public void ChildCollection()
        {
            var actualInstance = XamlLoader.Load(Dummy.ChildCollection);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.IsNotNull(dummyClass.Items);
            Assert.AreEqual(3, dummyClass.Items.Count);
        }

        [TestMethod]
        public void AttachedProperty()
        {
            var actualInstance = XamlLoader.Load(Dummy.AttachedProperty);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual(Container.GetProperty(dummyClass), "Value");
        }

        [TestMethod]
        public void Ignorable()
        {
            var actualInstance = XamlLoader.Load(Dummy.Ignorable);

            Assert.IsInstanceOfType(actualInstance, typeof (DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Property!", dummyClass.SampleProperty);
        }

        [TestMethod]        
        public void DirectiveInSpecialNamespaceThatIsNotX()
        {
            var actual = XamlLoader.Load(Dummy.KeyDirectiveNotInX);
            Assert.IsInstanceOfType(actual, typeof(DummyClass));
            var dictionary = (IDictionary)((DummyClass)actual).Resources;
            Assert.IsTrue(dictionary.Count > 0);
        }

        [TestMethod]
        [Ignore]
        public void PureCollection()
        {
            var actualInstance = XamlLoader.Load(Dummy.PureCollection);
        }
    }
}
