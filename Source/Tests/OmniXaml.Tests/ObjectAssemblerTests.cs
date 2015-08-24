namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Common.NetCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectAssembler;
    using ParsingSources;

    [TestClass]
    public class ObjectAssemblerTests : GivenAWiringContextWithNodeBuildersNetCore
    {
        private readonly XamlInstructionPack source;
        private readonly IObjectAssembler sut;

        public ObjectAssemblerTests()
        {
            sut = new ObjectAssembler(WiringContext, new TopDownMemberValueContext());
            source = new XamlInstructionPack(this);
        }

        [TestMethod]
        public void OneObject()
        {
            sut.PumpNodes(source.OneObject);

            var result = sut.Result;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
        }

        [TestMethod]
        public void ObjectWithMember()
        {
            sut.PumpNodes(source.ObjectWithMember);

            var result = sut.Result;
            var property = ((DummyClass) result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            Assert.AreEqual("Property!", property);
        }

        [TestMethod]
        public void ObjectWithTwoMembers()
        {
            sut.PumpNodes(source.ObjectWithTwoMembers);

            var result = sut.Result;
            var property1 = ((DummyClass) result).SampleProperty;
            var property2 = ((DummyClass) result).AnotherProperty;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            Assert.AreEqual("Property!", property1);
            Assert.AreEqual("Another!", property2);
        }

        [TestMethod]
        public void ObjectWithChild()
        {
            sut.PumpNodes(source.ObjectWithChild);

            var result = sut.Result;
            var property = ((DummyClass) result).Child;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            Assert.IsInstanceOfType(property, typeof (ChildClass));
        }

        [TestMethod]
        public void WithCollection()
        {
            sut.PumpNodes(source.CollectionWithMoreThanOneItem);

            var result = sut.Result;
            var children = ((DummyClass) result).Items;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            Assert.AreEqual(3, children.Count);
            CollectionAssert.AllItemsAreInstancesOfType(children, typeof (Item));
        }

        [TestMethod]
        public void CollectionWithInnerCollection()
        {
            sut.PumpNodes(source.CollectionWithInnerCollection);

            var result = sut.Result;
            var children = ((DummyClass) result).Items;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            Assert.AreEqual(3, children.Count);
            CollectionAssert.AllItemsAreInstancesOfType(children, typeof (Item));
            var innerCollection = children[0].Children;
            Assert.AreEqual(2, innerCollection.Count);
            CollectionAssert.AllItemsAreInstancesOfType(innerCollection, typeof (Item));
        }

        [TestMethod]
        public void WithCollectionAndInnerAttribute()
        {
            sut.PumpNodes(source.WithCollectionAndInnerAttribute);

            var result = sut.Result;
            var children = ((DummyClass) result).Items;
            var firstChild = children.First();
            var property = firstChild.Title;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            CollectionAssert.AllItemsAreInstancesOfType(children, typeof (Item));
            Assert.AreEqual("SomeText", property);
        }

        [TestMethod]
        public void MemberWithIncompatibleTypes()
        {
            sut.PumpNodes(source.GetMemberWithIncompatibleTypes());

            var result = sut.Result;
            var property = ((DummyClass) result).Number;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            Assert.AreEqual(12, property);
        }

        [TestMethod]
        public void ExtensionWithArgument()
        {
            sut.PumpNodes(source.ExtensionWithArgument);

            var result = sut.Result;
            var property = ((DummyClass) result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            Assert.AreEqual("Option", property);
        }

        [TestMethod]
        public void ExtensionWithTwoArguments()
        {
            sut.PumpNodes(source.ExtensionWithTwoArguments());

            var result = sut.Result;
            var property = ((DummyClass) result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            Assert.AreEqual("OneSecond", property);
        }

        [TestMethod]
        public void ExtensionWithNonStringArgument()
        {
            sut.PumpNodes(source.ExtensionWithNonStringArgument());

            var result = sut.Result;
            var property = ((DummyClass) result).Number;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            Assert.AreEqual(123, property);
        }

        [TestMethod]
        public void KeyDirective()
        {
            sut.PumpNodes(source.KeyDirective);

            var actual = sut.Result;
            Assert.IsInstanceOfType(actual, typeof (DummyClass));
            var dictionary = (IDictionary) ((DummyClass) actual).Resources;
            Assert.IsTrue(dictionary.Count > 0);
        }

        [TestMethod]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            sut.PumpNodes(source.GetString(sysNs));

            var actual = sut.Result;
            Assert.IsInstanceOfType(actual, typeof (string));
            Assert.AreEqual("Text", actual);
        }
    }
}