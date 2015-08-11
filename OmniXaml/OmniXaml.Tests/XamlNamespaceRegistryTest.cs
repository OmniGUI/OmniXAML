namespace OmniXaml.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Builder;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Typing;

    [TestClass]
    public class XamlNamespaceRegistryTest
    {
        private XamlNamespaceRegistry registry;
        private Type type;
        private string clrNamespace;

        [TestInitialize]
        public void Initialize()
        {
            type = typeof(DummyClass);
            registry = new XamlNamespaceRegistry();
            registry.RegisterPrefix(new PrefixRegistration("my", "target"));
            clrNamespace = $"clr-namespace:{type.Namespace};Assembly={type.GetTypeInfo().Assembly}";
            registry.RegisterPrefix(new PrefixRegistration("clr", clrNamespace));
            registry.AddNamespace(
                XamlNamespace.Map("target")
                    .With(new[] { Route.Assembly(type.Assembly).WithNamespaces(new[] { type.Namespace }) }));
        }

        [TestMethod]
        public void RegisterPrefixTest()
        {
            CollectionAssert.AreEqual(
                registry.RegisteredPrefixes.ToList(),
                new Collection<PrefixRegistration> {new PrefixRegistration("my", "target"), new PrefixRegistration("clr", clrNamespace)});
        }

        [TestMethod]
        public void GetClrNsByPrefix()
        {
            var clrNs = registry.GetClrNamespaceByPrefix("clr");
            var expected = new ClrNamespace(type.GetTypeInfo().Assembly, type.Namespace);
            Assert.AreEqual(expected, clrNs);            
        }

        [TestMethod]
        public void GetXamlNamespaceOfNotRegisteredPrefix()
        {
            Assert.IsNull(registry.GetXamlNamespace("unknown_namespace"));
        }

        [TestMethod]
        public void GetNamespaceByPrefix()
        {
            var ns = registry.GetXamlNamespaceByPrefix("my");
            Assert.AreEqual("target", ns.Name);
        }

        [TestMethod]
        public void GetNamespace()
        {            
            var ns = registry.GetXamlNamespace("target");
            Assert.AreEqual("target", ns.Name);
        }
    }
}
