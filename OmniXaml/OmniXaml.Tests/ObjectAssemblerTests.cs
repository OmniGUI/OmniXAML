namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NewAssembler;

    [TestClass]
    public class ObjectAssemblerTests : GivenAWiringContextWithNodeBuilders
    {
        private readonly IObjectAssembler sut;

        public ObjectAssemblerTests()
        {
            sut = new ObjectAssembler(WiringContext, new TopDownMemberValueContext());
        }

        [TestMethod]
        public void OneObject()
        {
            sut.PumpNodes(new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.EndObject(),
                });

            var result = sut.Result;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
        }

        [TestMethod]
        public void ObjectWithMember()
        {
            sut.PumpNodes(new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.Value("Property!"),
                    X.EndMember(),
                    X.EndObject(),
                });

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual("Property!", property);
        }

        [TestMethod]
        public void ObjectWithTwoMembers()
        {
            sut.PumpNodes(new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.Value("Property!"),
                    X.EndMember(),
                    X.StartMember<DummyClass>(d => d.AnotherProperty),
                    X.Value("Another!"),
                    X.EndMember(),
                    X.EndObject(),
                });

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
            sut.PumpNodes(new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Child),
                    X.StartObject<ChildClass>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                });

            var result = sut.Result;
            var property = ((DummyClass)result).Child;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.IsInstanceOfType(property, typeof(ChildClass));
        }

        [TestMethod]
        public void WithCollection()
        {
            sut.PumpNodes(new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                });

            var result = sut.Result;
            var children = ((DummyClass)result).Items;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual(3, children.Count);
            CollectionAssert.AllItemsAreInstancesOfType(children, typeof(Item));
        }

        [TestMethod]
        public void CollectionWithInnerCollection()
        {
            sut.PumpNodes(
                new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),


                    // Inner collection
                    X.StartMember<Item>(d => d.Children),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),                    
                    X.EndMember(),
                    X.EndObject(),


                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.StartObject<Item>(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                });

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
            sut.PumpNodes(new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Items),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<Item>(),
                    X.StartMember<Item>(d => d.Title),
                    X.Value("SomeText"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                });

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
            sut.PumpNodes(new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Number),
                    X.Value("12"),
                    X.EndMember(),
                    X.EndObject(),
                });

            var result = sut.Result;
            var property = ((DummyClass)result).Number;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual(12, property);
        }

        [TestMethod]
        public void ExtensionWithArgument()
        {
            sut.PumpNodes(
                new Collection<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject(typeof (DummyClass)),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.StartObject(typeof (DummyExtension)),
                    X.MarkupExtensionArguments(),
                    X.Value("Option"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                });

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual("Option", property);
        }

        [TestMethod]
        public void ExtensionWithTwoArguments()
        {
            sut.PumpNodes(
                new Collection<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject(typeof (DummyClass)),
                    X.StartMember<DummyClass>(d => d.SampleProperty),
                    X.StartObject(typeof (DummyExtension)),
                    X.MarkupExtensionArguments(),
                    X.Value("One"),
                    X.Value("Second"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                });

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual("OneSecond", property);
        }

        [TestMethod]
        public void ExtensionWithNonStringArgument()
        {
            sut.PumpNodes(
                new Collection<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(RootNs),
                    X.StartObject(typeof (DummyClass)),
                    X.StartMember<DummyClass>(d => d.Number),
                    X.StartObject(typeof (IntExtension)),
                    X.MarkupExtensionArguments(),
                    X.Value("123"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                });

            var result = sut.Result;
            var property = ((DummyClass)result).Number;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
            Assert.AreEqual(123, property);
        }

        [TestMethod]
        public void KeyDirective()
        {

            sut.PumpNodes(new Collection<XamlInstruction>
                {
                    X.StartObject<DummyClass>(),
                    X.StartMember<DummyClass>(d => d.Resources),
                    X.GetObject(),
                    X.Items(),
                    X.StartObject<ChildClass>(),
                    X.StartDirective("Key"),
                    X.Value("SomeKey"),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                    X.EndMember(),
                    X.EndObject(),
                });

            var actual = sut.Result;
            Assert.IsInstanceOfType(actual, typeof(DummyClass));
            var dictionary = (IDictionary)((DummyClass)actual).Resources;
            Assert.IsTrue(dictionary.Count > 0);
        }

        [TestMethod]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            sut.PumpNodes(
                new Collection<XamlInstruction>
                {
                    X.NamespacePrefixDeclaration(sysNs),
                    X.StartObject<string>(),
                    X.StartDirective("_Initialization"),
                    X.Value("Text"),
                    X.EndMember(),
                    X.EndObject(),
                });

            var actual = sut.Result;
            Assert.IsInstanceOfType(actual, typeof(string));
            Assert.AreEqual("Text", actual);
        }
    }
}
