namespace OmniXaml.Wpf
{
    using Typing;

    public class WpfXamlMember : XamlMember
    {
        public WpfXamlMember(string name) : base(name)
        {
        }

        public WpfXamlMember(string name, XamlType owner, IXamlTypeRepository mother, bool isAttachable) : base(name, owner, mother, isAttachable)
        {
        }
    }
}