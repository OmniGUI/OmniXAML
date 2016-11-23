namespace OmniXaml.Tests.XmlParserTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AnnotatorTests : XamlToTreeParserTestsBase
    {
        [TestMethod]
        public void NormalTest()
        {
            var xaml = @"<Window xmlns=""root"" xmlns:a=""one"" />";
            var p = ParseResult(xaml);
        }

        [TestMethod]
        public void NestTest()
        {
            var xaml = @"<Window xmlns=""root"">
<TextBlock xmlns:a=""one"" /></Window>";
            var p = ParseResult(xaml);
        }
    }
}