namespace OmniXaml.Wpf
{
    using Typing;

    public class WpfXamlAttachableMember : AttachableXamlMember
    {
        public WpfXamlAttachableMember(string name, WpfXamlType wpfXamlType, IXamlTypeRepository wiringContext, ITypeFactory typeFactory, ITypeFeatureProvider typeFeatureProvider) : base(name, wpfXamlType, wiringContext, typeFactory, typeFeatureProvider)
        {            
        }
    }
}