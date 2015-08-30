namespace OmniXaml.Typing
{
    using System.Reflection;
    using Glass;

    public class XamlMember : MutableXamlMember
    {
        public XamlMember(string name, XamlType declaringType, IXamlTypeRepository xamlTypeRepository, ITypeFeatureProvider featureProvider)
            : base(name, declaringType, xamlTypeRepository, featureProvider)
        {
            XamlType = LookupType();
        }

        public override bool IsAttachable => false;

        public override bool IsDirective => false;

        public override MethodInfo Getter => DeclaringType.UnderlyingType.GetRuntimeProperty(Name).GetMethod;
        public override MethodInfo Setter => DeclaringType.UnderlyingType.GetRuntimeProperty(Name).SetMethod;

        private XamlType LookupType()
        {
            var underlyingType = DeclaringType.UnderlyingType;
            var property = underlyingType.GetRuntimeProperty(Name);

            property.ThrowIfNull(() => new XamlParseException($"Cannot find a property named \"{Name}\" in the type {underlyingType}") );

            return TypeRepository.GetXamlType(property.PropertyType);
        }              
    }
}