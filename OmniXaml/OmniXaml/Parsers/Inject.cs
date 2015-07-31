namespace OmniXaml.Parsers
{
    using Glass;
    using Typing;

    internal static class Inject
    {
        public static XamlNode StartOfObject(XamlType xamlType)
        {
            Guard.ThrowIfNull(xamlType, nameof(xamlType));

            return new XamlNode(XamlNodeType.StartObject, xamlType);
        }

        public static XamlNode EndOfObject()
        {
            return new XamlNode(XamlNodeType.EndObject);
        }

        public static XamlNode PrefixDefinitionOfNamespace(ProtoXamlNode protoXamlNode)
        {
            var namespaceDeclaration = new NamespaceDeclaration(protoXamlNode.Namespace, protoXamlNode.Prefix);
            return new XamlNode(XamlNodeType.NamespaceDeclaration, namespaceDeclaration);
        }

        public static XamlNode StartOfMember(XamlMemberBase member)
        {
            return new XamlNode(XamlNodeType.StartMember, member);
        }

        public static XamlNode Value(string value)
        {
            return new XamlNode(XamlNodeType.Value, value);
        }

        public static XamlNode EndOfMember()
        {
            return new XamlNode(XamlNodeType.EndMember);
        }

        public static XamlNode GetObject()
        {
            return new XamlNode(XamlNodeType.GetObject);
        }

        public static XamlNode Items()
        {
            return new XamlNode(XamlNodeType.StartMember, CoreTypes.Items);
        }

        public static XamlNode MarkupExtensionArguments()
        {
            return new XamlNode(XamlNodeType.StartMember, CoreTypes.MarkupExtensionArguments);
        }
    }
}