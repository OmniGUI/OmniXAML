namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class AttributeDescriptor
    {
        public AttributeDescriptor(XamlType containingType, XamlType owner, string name)
        {
            this.ContainingType = containingType;
            this.Owner = owner;
            this.Name = name;
        }

        public XamlType ContainingType { get; }

        public XamlType Owner { get; }

        public string Name { get; }
    }
}