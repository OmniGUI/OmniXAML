namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class XamlAttribute
    {
        public XamlAttribute(UnboundAttribute unboundAttribute, XamlType type, IXamlTypeRepository typeContext)
        {
            Type = unboundAttribute.Type;
            Value = unboundAttribute.Value;
            Locator = unboundAttribute.Locator;

            Property = GetProperty(Locator, type, typeContext);
        }

        public string Value { get; private set; }

        public AttributeType Type { get; private set; }

        public PropertyLocator Locator { get; }

        public MutableXamlMember Property { get; private set; }

        private MutableXamlMember GetProperty(PropertyLocator propLocator, XamlType xamType, IXamlTypeRepository typingCore)
        {
            return propLocator.IsDotted ? GetAttachableMember(propLocator, typingCore) : GetRegularMember(xamType, typingCore);
        }

        private MutableXamlMember GetRegularMember(XamlType tagType, IXamlTypeRepository typeRepository)
        {
            return typeRepository.GetXamlType(tagType.UnderlyingType).GetMember(Locator.PropertyName);
        }

        private AttachableXamlMember GetAttachableMember(PropertyLocator memberLocator, IXamlTypeRepository typeRepository)
        {
            var owner = memberLocator.OwnerName;
            var ownerType = typeRepository.GetByPrefix(memberLocator.Prefix, owner);
            return typeRepository.GetXamlType(ownerType.UnderlyingType).GetAttachableMember(Locator.PropertyName);
        }
    }
}