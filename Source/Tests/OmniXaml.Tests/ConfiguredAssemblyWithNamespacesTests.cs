namespace OmniXaml.Tests
{
    using System.Reflection;
    using Builder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConfiguredAssemblyWithNamespacesTests
    {
        [TestMethod]
        public void LooksInCorrectNamespace()
        {
            var expectedType = typeof(OmniXaml.Tests.Classes.Another.DummyChild);
            var can = new ConfiguredAssemblyWithNamespaces(
                Assembly.GetAssembly(expectedType),
                new[] { "OmniXaml.Tests.Classes.Another" });

            var result = can.Get("DummyChild");

            Assert.AreEqual(expectedType, result);
        }
    }
}
