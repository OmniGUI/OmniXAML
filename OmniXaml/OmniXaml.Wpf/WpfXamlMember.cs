namespace OmniXaml.Wpf
{
    using Typing;

    public class WpfXamlMember : XamlMember
    {
        public WpfXamlMember(string name, XamlType declaringType, IXamlTypeRepository xamlTypeRepository, ITypeFeatureProvider typeFeatureProvider)
            : base(name, declaringType, xamlTypeRepository, typeFeatureProvider)
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