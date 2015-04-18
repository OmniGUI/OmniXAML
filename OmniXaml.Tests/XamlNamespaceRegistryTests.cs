namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Typing;

    [TestClass]
    public class XamlNamespaceRegistryTests
    {
        private XamlNamespaceRegistry registry;
        private IXamlTypeRepository xamlTypeRepository;

        [TestInitialize]
        public void Initialize()
        {
            registry = new XamlNamespaceRegistry();
            xamlTypeRepository = new XamlTypeRepository(new XamlNamespaceRegistry());
        }

        [TestMethod]
        public void RegisterPrefixTest()
        {
            registry.RegisterPrefix(new PrefixRegistration("my", "target"));
            CollectionAssert.AreEqual(registry.RegisteredPrefixes.ToList(), new List<string> { "my" });
        }

        [TestMethod]
        public void GetXamlNamespaceOfNotRegisteredPrefix()
        {
            Assert.IsNull(registry.GetXamlNamespace("namespace"));
        }

        [TestMethod]
        public void GetNamespaceByPrefix()
        {
            var type = typeof (DummyClass);

            registry.RegisterPrefix(new PrefixRegistration("my", "target"));
            registry.RegisterNamespace(
                new XamlNamespace(
                    "target",
                    new[] {new ClrAssemblyPair(type.Assembly, type.Namespace)}));

            var ns = registry.GetXamlNamespaceByPrefix("my");
            Assert.AreEqual("target", ns.NamespaceUri);
        }

        [TestMethod]
        public void GetNamespace()
        {
            var type = typeof (DummyClass);

            registry.RegisterPrefix(new PrefixRegistration("my", "target"));
            registry.RegisterNamespace(new XamlNamespace("target", new[] {new ClrAssemblyPair(type.Assembly, type.Namespace)}));
            var ns = registry.GetXamlNamespace("target");
            Assert.AreEqual("target", ns.NamespaceUri);
        }
    }
}
