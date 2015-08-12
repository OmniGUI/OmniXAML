namespace OmniXaml.Parsers.ProtoParser
{
    public enum NodeType
    {
        None,
        Element,
        EmptyElement,
        EndTag,
        PrefixDefinition,
        Directive,
        Attribute,
        Text,
        EmptyPropertyElement,
        PropertyElement
    }
}