namespace OmniXaml.Tests.XmlParserTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class DictionaryTests : XamlToTreeParserTestsBase
    {
        [TestMethod]
        public void Key()
        {
            var expected = new ConstructionNode(typeof(ResourceDictionary))
            {
                Children = new[] {new ConstructionNode(typeof(TextBlock)) {Key = "MyKey"},}
            };

            var actual = ParseResult(@"<ResourceDictionary xmlns:x=""special"" xmlns=""root""><TextBlock x:Key=""MyKey"" /></ResourceDictionary>");

            Assert.AreEqual(expected, actual.Root);
        }
    }
}