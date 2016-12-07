namespace OmniXaml.Tests.XmlParserTests
{
    using Xunit;

    public class AnnotatorTests : XamlToTreeParserTestsBase
    {
        [Fact]
        public void NormalTest()
        {
            var xaml = @"<Window xmlns=""root"" xmlns:a=""one"" />";
            var p = ParseResult(xaml);
        }

        [Fact]
        public void NestTest()
        {
            var xaml = @"<Window xmlns=""root"">
<TextBlock xmlns:a=""one"" /></Window>";
            var p = ParseResult(xaml);
        }
    }
}