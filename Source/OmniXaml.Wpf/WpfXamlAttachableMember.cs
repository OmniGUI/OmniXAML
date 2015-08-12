namespace OmniXaml.Wpf
{
    using System.Reflection;
    using Typing;

    public class WpfXamlAttachableMember : AttachableXamlMember
    {
        public WpfXamlAttachableMember(string name,
            MethodInfo getter,
            MethodInfo setter,
            IXamlTypeRepository xamlTypeRepository,
            ITypeFeatureProvider typeFeatureProvider)
            : base(name, getter, setter, xamlTypeRepository, typeFeatureProvider)
        {
        }
    }
}