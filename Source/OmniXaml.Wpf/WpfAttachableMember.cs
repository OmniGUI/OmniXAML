namespace OmniXaml.Wpf
{
    using System.Reflection;
    using Typing;

    public class WpfAttachableMember : AttachableMember
    {
        public WpfAttachableMember(string name,
            MethodInfo getter,
            MethodInfo setter,
            ITypeRepository typeRepository,
            ITypeFeatureProvider typeFeatureProvider)
            : base(name, getter, setter, typeRepository, typeFeatureProvider)
        {
        }
    }
}