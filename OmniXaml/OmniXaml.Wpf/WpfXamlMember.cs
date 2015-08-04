namespace OmniXaml.Wpf
{
    using Typing;

    public class WpfXamlMember : XamlMember
    {
        public WpfXamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory, ITypeFeatureProvider typeFeatureProvider)
            : base(name, owner, xamlTypeRepository, typeFactory, typeFeatureProvider)
        {
        }

        protected override IXamlMemberValuePlugin LookupXamlMemberValueConnector()
        {
            return new MemberValuePlugin(this);
        }

        public override bool IsDirective => false;
        public override bool IsAttachable => false;
    }
}