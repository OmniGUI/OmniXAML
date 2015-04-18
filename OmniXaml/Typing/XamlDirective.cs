namespace OmniXaml.Typing
{
    public class XamlDirective : XamlMember
    {
        public XamlDirective(string name)
            : base(name)
        {
            this.IsDirective = true;
        }

        public XamlDirective(string name, XamlType xamlType) : this(name)
        {
            this.Type = xamlType;
        }
    }
}