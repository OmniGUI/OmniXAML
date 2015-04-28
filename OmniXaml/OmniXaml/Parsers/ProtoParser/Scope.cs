namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class Scope
    {
        public Scope(XamlType xamlType, string ns)
        {
            XamlType = xamlType;
            Namespace = ns;
        }

        public XamlType XamlType { get; private set; }

        public XamlMember XamlProperty { get; set; }

        public bool IsPreservingSpace { get; set; }

        public bool IsInsideContent { get; set; }

        public string Namespace { get; private set; }
    }
}