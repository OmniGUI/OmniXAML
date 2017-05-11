using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests.XmlToNodes
{
    public class DictionaryTests : XamlToTreeParserTestsBase
    {
        [Fact]
        public void Key()
        {
            var expected = new ConstructionNode(typeof(ResourceDictionary))
            {
                Children = new[] {new ConstructionNode(typeof(TextBlock)) {Key = "MyKey"},}
            };

            var actual = ParseResult(@"<ResourceDictionary xmlns:x=""special"" xmlns=""root""><TextBlock x:Key=""MyKey"" /></ResourceDictionary>");

            Assert.Equal(expected, actual.Root);
        }
    }
}