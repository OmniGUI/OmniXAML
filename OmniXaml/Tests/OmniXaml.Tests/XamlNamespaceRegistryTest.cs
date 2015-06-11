namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Builder;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Typing;

    [TestClass]
    public class XamlNamespaceRegistryTest
    {
        private XamlNamespaceRegistry registry;

        [TestInitialize]
        public void Initialize()
        {
            var type = typeof(DummyClass);
            registry = new XamlNamespaceRegistry();
            registry.RegisterPrefix(new PrefixRegistration("my", "target"));
            registry.AddNamespace(
                XamlNamespace.Map("target")
                    .With(new[] { Route.Assembly(type.Assembly).WithNamespaces(new[] { type.Namespace }) }));
        }

        [TestMethod]
        public void RegisterPrefixTest()
        {
            CollectionAssert.AreEqual(registry.RegisteredPrefixes.ToList(), new List<string> { "my" });
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
