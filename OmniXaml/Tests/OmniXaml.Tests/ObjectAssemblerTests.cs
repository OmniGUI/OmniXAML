namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Builder;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NewAssembler;

    [TestClass]
    public class ObjectAssemblerTests : GivenAWiringContext
    {
        private readonly XamlInstructionBuilder builder;
        private readonly IObjectAssembler sut;

        public ObjectAssemblerTests()
        {
            builder = new XamlInstructionBuilder(WiringContext.TypeContext);
            sut = new ObjectAssembler(WiringContext, new TopDownMemberValueContext());
        }

        [TestMethod]
        public void OneObject()
        {
            sut.PumpNodes(new Collection<XamlInstruction>
                {
                    builder.StartObject<DummyClass>(),
                    builder.EndObject(),
                });

            var result = sut.Result;

            Assert.IsInstanceOfType(result, typeof(DummyClass));
        }

        [TestMethod]
        public void ObjectWithMember()
        {
            sut.PumpNodes(new Collection<XamlInstruction>
                {
                    builder.StartObject<DummyClass>(),
                    builder.StartMember<DummyClass>(d => d.SampleProperty),
                    builder.Value("Property!"),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.StartObject<DummyClass>(),
                    builder.StartMember<DummyClass>(d => d.SampleProperty),
                    builder.Value("Property!"),
                    builder.EndMember(),
                    builder.StartMember<DummyClass>(d => d.AnotherProperty),
                    builder.Value("Another!"),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.StartObject<DummyClass>(),
                    builder.StartMember<DummyClass>(d => d.Child),
                    builder.StartObject<ChildClass>(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.StartObject<DummyClass>(),
                    builder.StartMember<DummyClass>(d => d.Items),
                    builder.GetObject(),
                    builder.Items(),
                    builder.StartObject<Item>(),
                    builder.EndObject(),
                    builder.StartObject<Item>(),
                    builder.EndObject(),
                    builder.StartObject<Item>(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.StartObject<DummyClass>(),
                    builder.StartMember<DummyClass>(d => d.Items),
                    builder.GetObject(),
                    builder.Items(),
                    builder.StartObject<Item>(),


                    // Inner collection
                    builder.StartMember<Item>(d => d.Children),
                    builder.GetObject(),
                    builder.Items(),
                    builder.StartObject<Item>(),
                    builder.EndObject(),
                    builder.StartObject<Item>(),
                    builder.EndObject(),                    
                    builder.EndMember(),
                    builder.EndObject(),


                    builder.EndObject(),
                    builder.StartObject<Item>(),
                    builder.EndObject(),
                    builder.StartObject<Item>(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.StartObject<DummyClass>(),
                    builder.StartMember<DummyClass>(d => d.Items),
                    builder.GetObject(),
                    builder.Items(),
                    builder.StartObject<Item>(),
                    builder.StartMember<Item>(d => d.Title),
                    builder.Value("SomeText"),
                    builder.EndMember(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.StartObject<DummyClass>(),
                    builder.StartMember<DummyClass>(d => d.Number),
                    builder.Value("12"),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.NamespacePrefixDeclaration(RootNs),
                    builder.StartObject(typeof (DummyClass)),
                    builder.StartMember<DummyClass>(d => d.SampleProperty),
                    builder.StartObject(typeof (DummyExtension)),
                    builder.MarkupExtensionArguments(),
                    builder.Value("Option"),
                    builder.EndMember(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.NamespacePrefixDeclaration(RootNs),
                    builder.StartObject(typeof (DummyClass)),
                    builder.StartMember<DummyClass>(d => d.SampleProperty),
                    builder.StartObject(typeof (DummyExtension)),
                    builder.MarkupExtensionArguments(),
                    builder.Value("One"),
                    builder.Value("Second"),
                    builder.EndMember(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.NamespacePrefixDeclaration(RootNs),
                    builder.StartObject(typeof (DummyClass)),
                    builder.StartMember<DummyClass>(d => d.Number),
                    builder.StartObject(typeof (IntExtension)),
                    builder.MarkupExtensionArguments(),
                    builder.Value("123"),
                    builder.EndMember(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.StartObject<DummyClass>(),
                    builder.StartMember<DummyClass>(d => d.Resources),
                    builder.GetObject(),
                    builder.Items(),
                    builder.StartObject<ChildClass>(),
                    builder.StartDirective("Key"),
                    builder.Value("SomeKey"),
                    builder.EndMember(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
                    builder.EndMember(),
                    builder.EndObject(),
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
                    builder.NamespacePrefixDeclaration(sysNs),
                    builder.StartObject<string>(),
                    builder.StartDirective("_Initialization"),
                    builder.Value("Text"),
                    builder.EndMember(),
                    builder.EndObject(),
                });

            var actual = sut.Result;
            Assert.IsInstanceOfType(actual, typeof(string));
            Assert.AreEqual("Text", actual);
        }
    }
}
