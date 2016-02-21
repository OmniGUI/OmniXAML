namespace OmniXaml.Parsers.MarkupExtensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Typing;

    public class MarkupExtensionNodeToXamlNodesConverter
    {
        public MarkupExtensionNodeToXamlNodesConverter(IRuntimeTypeSource typeSource)
        {
            TypeSource = typeSource;
        }

        public IEnumerable<Instruction> ParseMarkupExtensionNode(MarkupExtensionNode tree)
        {
            var identifierNode = tree.Identifier;
            var xamlType = TypeSource.GetByPrefix(identifierNode.Prefix, identifierNode.TypeName);
            yield return Inject.StartOfObject(xamlType);

            foreach (var instruction in ParseArguments(tree.Options.OfType<PositionalOption>())) yield return instruction;
            foreach (var instruction in ParseProperties(tree.Options.OfType<PropertyOption>(), xamlType)) yield return instruction;

            yield return Inject.EndOfObject();
        }

        private IRuntimeTypeSource TypeSource { get; }

        private static IEnumerable<Instruction> ParseArguments(IEnumerable<PositionalOption> options)
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

        private IEnumerable<Instruction> ParseProperties(IEnumerable<PropertyOption> options, XamlType xamlType)
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

                var markupExtensionNode = option.Value as MarkupExtensionNode;
                if (markupExtensionNode != null)
                {
                    foreach (var xamlInstruction in ParseMarkupExtensionNode(markupExtensionNode)) { yield return xamlInstruction; }
                }

                yield return Inject.EndOfMember();
            }
        }
    }
}