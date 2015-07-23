namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class ProtoParserNode
    {
        public string Prefix { get; set; }

        public bool IsEmptyTag { get; set; }

        public XamlType Type { get; set; }

        public NodeType NodeType { get; set; }

        public string TypeNamespace { get; set; }

        public MutableXamlMember PropertyAttribute { get; set; }

        public TextBuffer PropertyAttributeText { get; set; }

        public XamlMember PropertyElement { get; set; }

        public TextBuffer TextContent { get; set; }
    }
}