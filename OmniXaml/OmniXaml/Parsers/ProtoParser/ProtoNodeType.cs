namespace OmniXaml.Parsers.ProtoParser
{
    public enum ProtoNodeType
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