namespace OmniXaml.Parsers.ProtoParser
{
    internal enum AttributeType
    {
        Namespace,
        Property,
        CtorDirective,
        Name,
        Directive,
        // ReSharper disable once UnusedMember.Global
        XmlSpace,
        // ReSharper disable once UnusedMember.Global
        Event,
        AttachableProperty,
        // ReSharper disable once UnusedMember.Global
        Unknown
    }
}