namespace OmniXaml.Wpf
{
    using System.Xaml;

    public class DirectiveAdapter : XamlDirective
    {
        public DirectiveAdapter(XamlDirective xamlMember)
            : base(xamlMember.GetXamlNamespaces(), xamlMember.Name, xamlMember.Type, xamlMember.TypeConverter, xamlMember.AllowedLocation)
        {
        }
    }
}