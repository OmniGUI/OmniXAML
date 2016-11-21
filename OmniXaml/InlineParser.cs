namespace OmniXaml
{
    using System;
    using InlineParsers.Extensions;
    using Sprache;
    using Tests.Namespaces;

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

        public ConstructionNode Parse(string inline, Func<string, string> resolver)
        {
            var tree = MarkupExtensionParser.MarkupExtension.Parse(inline);
            
            var markupExtensionNodeToConstructionNodeConverter = new MarkupExtensionNodeToConstructionNodeConverter(typeDirectory, resolver);
            return markupExtensionNodeToConstructionNodeConverter.Convert(tree);
        }
    }
}