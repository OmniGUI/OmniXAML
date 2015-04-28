namespace OmniXaml.Typing
{
    public class XamlDirective : XamlMember
    {
        public XamlDirective(string name)
            : base(name)
        {
            IsDirective = true;
        }

        public XamlDirective(string name, XamlType xamlType) : this(name)
        {
            Type = xamlType;
        }
    }
}