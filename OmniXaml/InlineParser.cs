namespace OmniXaml
{
    using InlineParsers.Extensions;
    using Sprache;

    public class InlineParser : IInlineParser
    {
        private readonly ITypeDirectory typeDirectory;

        public InlineParser(ITypeDirectory typeDirectory)
        {
            this.typeDirectory = typeDirectory;
        }

        public bool CanParse(string inline)
        {
            return inline.StartsWith("{") && inline.EndsWith("}");
        }

        public ConstructionNode Parse(string inline)
        {
            var tree = MarkupExtensionParser.MarkupExtension.Parse(inline);
            var markupExtensionNodeToConstructionNodeConverter = new MarkupExtensionNodeToConstructionNodeConverter(typeDirectory);
            return markupExtensionNodeToConstructionNodeConverter.Convert(tree);
        }
    }
}