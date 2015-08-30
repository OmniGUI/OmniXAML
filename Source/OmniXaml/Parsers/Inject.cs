namespace OmniXaml.Parsers
{
    using Glass;
    using Typing;

    internal static class Inject
    {
        public static XamlInstruction StartOfObject(XamlType xamlType)
        {
            Guard.ThrowIfNull(xamlType, nameof(xamlType));

            return new XamlInstruction(XamlInstructionType.StartObject, xamlType);
        }

        public static XamlInstruction EndOfObject()
        {
            return new XamlInstruction(XamlInstructionType.EndObject);
        }

        public static XamlInstruction PrefixDefinitionOfNamespace(ProtoXamlInstruction protoXamlInstruction)
        {
            var namespaceDeclaration = new NamespaceDeclaration(protoXamlInstruction.Namespace, protoXamlInstruction.Prefix);
            return new XamlInstruction(XamlInstructionType.NamespaceDeclaration, namespaceDeclaration);
        }

        public static XamlInstruction StartOfMember(XamlMemberBase member)
        {
            return new XamlInstruction(XamlInstructionType.StartMember, member);
        }

        public static XamlInstruction Value(string value)
        {
            return new XamlInstruction(XamlInstructionType.Value, value);
        }

        public static XamlInstruction EndOfMember()
        {
            return new XamlInstruction(XamlInstructionType.EndMember);
        }

        public static XamlInstruction GetObject()
        {
            return new XamlInstruction(XamlInstructionType.GetObject);
        }

        public static XamlInstruction Items()
        {
            return new XamlInstruction(XamlInstructionType.StartMember, CoreTypes.Items);
        }

        public static XamlInstruction MarkupExtensionArguments()
        {
            return new XamlInstruction(XamlInstructionType.StartMember, CoreTypes.MarkupExtensionArguments);
        }

        public static XamlInstruction Initialization()
        {
            return new XamlInstruction(XamlInstructionType.StartMember, CoreTypes.Initialization);
        }
    }
}