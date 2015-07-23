namespace OmniXaml.Wpf
{
    using Typing;

    public class WpfXamlAttachableMember : AttachableXamlMember
    {
        public WpfXamlAttachableMember(string name, WpfXamlType wpfXamlType, IXamlTypeRepository wiringContext, ITypeFactory typeFactory) : base(name, wpfXamlType, wiringContext, typeFactory)
        {            
        }
    }
}