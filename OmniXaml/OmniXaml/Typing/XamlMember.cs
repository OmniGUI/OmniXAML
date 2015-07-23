namespace OmniXaml.Typing
{
    using System.Reflection;

    public class XamlMember : MutableXamlMember
    {
        public XamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory)
            : base(name, owner, xamlTypeRepository, typeFactory)
        {
        }

        public override bool IsAttachable => false;

        public override bool IsDirective => false;
        protected override XamlType LookupType()
        {
            var property = RuntimeReflectionExtensions.GetRuntimeProperty(DeclaringType.UnderlyingType, Name);
            return XamlType.Create(property.PropertyType, TypeRepository, TypeFactory);
        }
    }
}