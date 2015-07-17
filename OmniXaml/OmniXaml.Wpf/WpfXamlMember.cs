namespace OmniXaml.Wpf
{
    using Typing;

    public class WpfXamlMember : XamlMember
    {
        public WpfXamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory, bool isAttachable)
            : base(name, owner, xamlTypeRepository, typeFactory, isAttachable)
        {
        }

        protected override IXamlMemberValuePlugin LookupXamlMemberValueConnector()
        {
            return new WpfXamlMemberValueConnector(this);
        }
    }
}