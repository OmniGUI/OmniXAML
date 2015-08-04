namespace OmniXaml.Typing
{
    using System.Reflection;

    public class XamlMember : MutableXamlMember
    {
        private readonly ITypeFeatureProvider featureProvider;

        public XamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory, ITypeFeatureProvider featureProvider)
            : base(name, owner, xamlTypeRepository, typeFactory)
        {
            this.featureProvider = featureProvider;
        }

        public override bool IsAttachable => false;

        public override bool IsDirective => false;
        protected override XamlType LookupType()
        {
            var property = RuntimeReflectionExtensions.GetRuntimeProperty(DeclaringType.UnderlyingType, Name);
            return XamlType.Create(property.PropertyType, TypeRepository, TypeFactory, featureProvider);
        }
    }
}