namespace OmniXaml.Parsers
{
    using Glass.Core;
    using Typing;

    internal static class Inject
    {
        public static Instruction StartOfObject(XamlType xamlType)
        {
            Guard.ThrowIfNull(xamlType, nameof(xamlType));

            return new Instruction(InstructionType.StartObject, xamlType);
        }

        public static Instruction EndOfObject()
        {
            return new Instruction(InstructionType.EndObject);
        }

        public static Instruction PrefixDefinitionOfNamespace(ProtoInstruction protoInstruction)
        {
            var namespaceDeclaration = new NamespaceDeclaration(protoInstruction.Namespace, protoInstruction.Prefix);
            return new Instruction(InstructionType.NamespaceDeclaration, namespaceDeclaration);
        }

        public static Instruction StartOfMember(MemberBase member)
        {
            return new Instruction(InstructionType.StartMember, member);
        }

        public static Instruction Value(string value)
        {
            return new Instruction(InstructionType.Value, value);
        }

        public static Instruction EndOfMember()
        {
            return new Instruction(InstructionType.EndMember);
        }

        public static Instruction GetObject()
        {
            return new Instruction(InstructionType.GetObject);
        }

        public static Instruction Items()
        {
            return new Instruction(InstructionType.StartMember, CoreTypes.Items);
        }

        public static Instruction MarkupExtensionArguments()
        {
            return new Instruction(InstructionType.StartMember, CoreTypes.MarkupExtensionArguments);
        }

        public static Instruction Initialization()
        {
            return new Instruction(InstructionType.StartMember, CoreTypes.Initialization);
        }

        public static Instruction UnknownContent()
        {
            return new Instruction(InstructionType.StartMember, CoreTypes.UnknownContent);
        }
    }
}