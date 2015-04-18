namespace OmniXaml
{
    using Typing;

    public interface IXamlReader
    {
        XamlNodeType NodeType { get; }

        bool IsEof { get; }
        XamlType Type { get; }
        XamlMember Member { get; }
        NamespaceDeclaration Namespace { get; }
        object Value { get; }

        bool Read();
    }
}