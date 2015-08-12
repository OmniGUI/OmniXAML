namespace OmniXaml.Parsers
{
    using Glass;
    using Typing;

    internal static class Inject
    {
        public static XamlInstruction StartOfObject(XamlType xamlType)
        {
            Guard.ThrowIfNull(xamlType, nameof(xamlType));

            return new XamlInstruction(XamlNodeType.StartObject, xamlType);
        }

        public static XamlInstruction EndOfObject()
        {
            return new XamlInstruction(XamlNodeType.EndObject);
        }

        public static XamlInstruction PrefixDefinitionOfNamespace(ProtoXamlInstruction protoXamlInstruction)
        {
            var namespaceDeclaration = new NamespaceDeclaration(protoXamlInstruction.Namespace, protoXamlInstruction.Prefix);
            return new XamlInstruction(XamlNodeType.NamespaceDeclaration, namespaceDeclaration);
        }

        public static XamlInstruction StartOfMember(XamlMemberBase member)
        {
            return new XamlInstruction(XamlNodeType.StartMember, member);
        }

        public static XamlInstruction Value(string value)
        {
            return new XamlInstruction(XamlNodeType.Value, value);
        }

        public static XamlInstruction EndOfMember()
        {
            return new XamlInstruction(XamlNodeType.EndMember);
        }

        public static XamlInstruction GetObject()
        {
            return new XamlInstruction(XamlNodeType.GetObject);
        }

        public static XamlInstruction Items()
        {
            return new XamlInstruction(XamlNodeType.StartMember, CoreTypes.Items);
        }

        public static XamlInstruction MarkupExtensionArguments()
        {
            return new XamlInstruction(XamlNodeType.StartMember, CoreTypes.MarkupExtensionArguments);
        }

        public static XamlInstruction Initialization()
        {
            return new XamlInstruction(XamlNodeType.StartMember, CoreTypes.Initialization);
        }
    }
}