using OmniXaml.Tests.Model;
using Xunit;

namespace OmniXaml.Tests.XmlToNodes
{
    public class DictionaryTests : XamlToTreeParserTestsBase
    {
        [Fact]
        public void Key()
        {
            var children = new[] {new ConstructionNode<TextBlock> {Key = "MyKey"},};
            var expected = new ConstructionNode<ResourceDictionary>()
                .WithChildren(children);

            var actual = ParseResult(@"<ResourceDictionary xmlns:x=""special"" xmlns=""root""><TextBlock x:Key=""MyKey"" /></ResourceDictionary>");

            Assert.Equal(expected, actual.Root);
        }
    }
}