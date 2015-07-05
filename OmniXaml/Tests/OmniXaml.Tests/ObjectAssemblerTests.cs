namespace OmniXaml.Tests
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract class ObjectAssemblerTests : GivenAWiringContext
    {
        private readonly XamlNodeBuilder builder;
        private readonly IObjectAssembler sut;

        protected ObjectAssemblerTests()
        {
            builder = new XamlNodeBuilder(WiringContext.TypeContext);
            sut = CreateObjectAssembler();
        }

        protected abstract IObjectAssembler CreateObjectAssembler();       

        [TestMethod]
        public void OneObject()
        {
            sut.PumpNodes(new Collection<XamlNode>
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
            sut.PumpNodes(new Collection<XamlNode>
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
            sut.PumpNodes(new Collection<XamlNode>
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
            sut.PumpNodes(new Collection<XamlNode>
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
            sut.PumpNodes(new Collection<XamlNode>
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
        public void WithCollectionAndInnerAttribute()
        {
            sut.PumpNodes(new Collection<XamlNode>
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
            sut.PumpNodes(new Collection<XamlNode>
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
                new Collection<XamlNode>
                {
                    builder.NamespacePrefixDeclaration("root", ""),
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
            var property = ((DummyClass) result).SampleProperty;

            Assert.IsInstanceOfType(result, typeof (DummyClass));
            Assert.AreEqual("Option", property);
        }

        [TestMethod]
        public void ExtensionWithTwoArguments()
        {
            sut.PumpNodes(
                new Collection<XamlNode>
                {
                    builder.NamespacePrefixDeclaration("root", ""),
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
                new Collection<XamlNode>
                {
                    builder.NamespacePrefixDeclaration("root", ""),
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
    }
}
