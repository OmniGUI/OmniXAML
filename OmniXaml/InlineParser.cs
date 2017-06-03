namespace OmniXaml
{
    using System;
    using InlineParsers.Extensions;
    using Sprache;

    public class InlineParser : IInlineParser
    {
        private readonly IXmlTypeResolver xmlTypeResolver;

        public InlineParser(IXmlTypeResolver xmlTypeResolver)
        {
            this.xmlTypeResolver = xmlTypeResolver;
        }

        public bool CanParse(string inline)
        {
            return inline.StartsWith("{") && inline.EndsWith("}");
        }

        public ConstructionNode Parse(string inline, Func<string, string> prefixResolver)
        {
            var tree = MarkupExtensionParser.MarkupExtension.Parse(inline);
            
            var markupExtensionNodeToConstructionNodeConverter = new MarkupExtensionNodeToConstructionNodeConverter(prefixResolver, xmlTypeResolver);
            return markupExtensionNodeToConstructionNodeConverter.Convert(tree);
        }
    }
}