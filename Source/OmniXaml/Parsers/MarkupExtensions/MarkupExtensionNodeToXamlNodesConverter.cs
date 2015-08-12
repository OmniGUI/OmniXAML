namespace OmniXaml.Parsers.MarkupExtensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Typing;

    public class MarkupExtensionNodeToXamlNodesConverter
    {
        private readonly WiringContext wiringContext;

        public MarkupExtensionNodeToXamlNodesConverter(WiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IEnumerable<XamlInstruction> Convert(MarkupExtensionNode tree)
        {
            var identifierNode = tree.Identifier;
            var xamlType = wiringContext.TypeContext.GetByPrefix(identifierNode.Prefix, identifierNode.TypeName);
            yield return Inject.StartOfObject(xamlType);

            foreach (var xamlNode in ParseArguments(tree.Options.OfType<PositionalOption>())) yield return xamlNode;
            foreach (var xamlNode in ParseProperties(tree.Options.OfType<PropertyOption>(), xamlType)) yield return xamlNode;

            yield return Inject.EndOfObject();
        }

        private static IEnumerable<XamlInstruction> ParseArguments(IEnumerable<PositionalOption> options)
        {
            var positionalOptions = options.ToList();
            if (positionalOptions.Any())
            {
                yield return Inject.MarkupExtensionArguments();
                foreach (var option in positionalOptions)
                {
                    yield return Inject.Value(option.Identifier);
                }
                yield return Inject.EndOfMember();
            }
        }

        private static IEnumerable<XamlInstruction> ParseProperties(IEnumerable<PropertyOption> options, XamlType xamlType)
        {
            foreach (var option in options)
            {
                var member = xamlType.GetMember(option.Property);
                yield return Inject.StartOfMember(member);

                var stringNode = option.Value as StringNode;
                if (stringNode != null)
                {
                    yield return Inject.Value(stringNode.Value);
                }

                yield return Inject.EndOfMember();
            }
        }
    }
}