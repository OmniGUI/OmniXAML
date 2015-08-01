namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using System.Collections;
    using System.Diagnostics;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xaml.Tests.Resources;

    [TestClass]
    public class ChildInstanceReadingTests : GivenAXamlXmlLoader
    {
        [TestMethod]
        public void ReadInstanceWithChild()
        {
            var actualInstance = XamlStreamLoader.Load(Dummy.InstanceWithChild);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Debug.Assert(dummyClass != null, "dummyClass != null");            
            Assert.IsInstanceOfType(dummyClass.Child, typeof(ChildClass));
        }


        [TestMethod]
        public void ReadInstanceWithThreeLevelsOfNesting()
        {
            var root = XamlStreamLoader.Load(Dummy.ThreeLevelsOfNesting);
            
            var dummy = root as DummyClass;
            Assert.IsInstanceOfType(root, typeof(DummyClass), "The retrieved instance should be of type DummyClass");

            Debug.Assert(dummy != null, "dummy != null");
            var level2Instance = dummy.Child;
            Assert.IsNotNull(level2Instance);

            var level3Instance = level2Instance.Child;
            Assert.IsNotNull(level3Instance);
        }

        [TestMethod]
        public void KeyDirective()
        {
            var actual = XamlStreamLoader.Load(Dummy.KeyDirective);
            Assert.IsInstanceOfType(actual, typeof(DummyClass));
            var dictionary = (IDictionary)((DummyClass)actual).Resources;
            Assert.IsTrue(dictionary.Count > 0);
        }
    }
}