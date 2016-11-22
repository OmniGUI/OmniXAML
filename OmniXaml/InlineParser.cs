namespace OmniXaml
{
    using System;
    using InlineParsers.Extensions;
    using Sprache;
    using Tests.Namespaces;

    public class InlineParser : IInlineParser
    {
        private readonly ITypeDirectory typeDirectory;
        private readonly IResolver resolver;

        public InlineParser(ITypeDirectory typeDirectory, IResolver resolver)
        {
            this.typeDirectory = typeDirectory;
            this.resolver = resolver;
        }

        public bool CanParse(string inline)
        {
            return inline.StartsWith("{") && inline.EndsWith("}");
        }

        public ConstructionNode Parse(string inline, Func<string, string> prefixResolver)
        {
            var tree = MarkupExtensionParser.MarkupExtension.Parse(inline);
            
            var markupExtensionNodeToConstructionNodeConverter = new MarkupExtensionNodeToConstructionNodeConverter(prefixResolver, resolver);
            return markupExtensionNodeToConstructionNodeConverter.Convert(tree);
        }
    }
}