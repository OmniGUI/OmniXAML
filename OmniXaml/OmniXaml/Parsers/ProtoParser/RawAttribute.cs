namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class RawAttribute
    {
        public XamlType ContainingType { get; }
        public XamlType Owner { get; }
        public string Name { get; }
        public string Value { get; }

        public RawAttribute(AttributeDescriptor attributeDescriptor, string value)
        {
            ContainingType = attributeDescriptor.ContainingType;
            Owner = attributeDescriptor.Owner;
            Name = attributeDescriptor.Name;
            Value = value;
        }
    }
}