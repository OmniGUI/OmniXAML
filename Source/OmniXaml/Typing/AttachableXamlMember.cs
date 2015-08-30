namespace OmniXaml.Typing
{
    using System.Reflection;

    public class AttachableXamlMember : MutableXamlMember
    {
        private readonly MethodInfo setter;
        private readonly MethodInfo getter;

        public AttachableXamlMember(string name,
            MethodInfo getter,
            MethodInfo setter,
            IXamlTypeRepository xamlTypeRepository,
            ITypeFeatureProvider featureProvider) : base(name, xamlTypeRepository.GetXamlType(getter.DeclaringType), xamlTypeRepository, featureProvider)
        {
            this.getter = getter;
            this.setter = setter;
            XamlType = LookupType();
        }

        public override bool IsAttachable => true;
        public override bool IsDirective => false;

        public override MethodInfo Getter => getter;

        public override MethodInfo Setter => setter;

        private XamlType LookupType()
        {
            return TypeRepository.GetXamlType(getter.ReturnType);
        }
    }
}