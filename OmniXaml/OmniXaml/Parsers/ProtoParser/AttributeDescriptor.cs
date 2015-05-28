namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class AttributeDescriptor
    {
        public PropertyLocator Locator { get; }

        public AttributeDescriptor(PropertyLocator propertyLocator, XamlType containingType, XamlType owner, string name)
        {
            Locator = propertyLocator;
            ContainingType = containingType;
            Owner = owner;
            Name = name;
        }

        public XamlType ContainingType { get; }

        public XamlType Owner { get; }

        public string Name { get; }
    }
}