namespace OmniXaml.Typing
{
    using System.Reflection;

    public class XamlMember : MutableXamlMember
    {
        private readonly ITypeFeatureProvider featureProvider;

        public XamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory, ITypeFeatureProvider featureProvider)
            : base(name, owner, xamlTypeRepository, typeFactory, featureProvider)
        {
        }

        public override bool IsAttachable => false;

        public override bool IsDirective => false;

        protected override XamlType LookupType()
        {
            var property = DeclaringType.UnderlyingType.GetRuntimeProperty(Name);
            return XamlType.Create(property.PropertyType, TypeRepository, TypeFactory, FeatureProvider);
        }
    }
}