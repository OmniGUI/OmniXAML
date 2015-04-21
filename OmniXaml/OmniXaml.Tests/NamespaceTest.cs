namespace OmniXaml.Tests
{
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Typing;

    [TestClass]
    public class NamespaceTest
    {
        [TestMethod]
        public void TryGetType()
        {
            var expectedType = typeof(DummyClass);

            var ns = new XamlNamespace("mynamespace");

            ns.AddMapping(new ClrAssemblyPair(expectedType.Assembly, expectedType.Namespace));

            var actualType = ns.GetXamlType(expectedType.Name);

            Assert.AreEqual(expectedType, actualType.UnderlyingType);
        }        
    }
}
