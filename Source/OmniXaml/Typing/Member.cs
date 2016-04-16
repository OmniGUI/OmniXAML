namespace OmniXaml.Typing
{
    using System.Reflection;
    using Glass.Core;

    public class Member : MutableMember
    {
        public Member(string name, XamlType declaringType, ITypeRepository typeRepository, ITypeFeatureProvider featureProvider)
            : base(name, declaringType, typeRepository, featureProvider)
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

            property.ThrowIfNull(() => new ParseException($"Cannot find a property named \"{Name}\" in the type {underlyingType}") );

            return TypeRepository.GetByType(property.PropertyType);
        }              
    }
}