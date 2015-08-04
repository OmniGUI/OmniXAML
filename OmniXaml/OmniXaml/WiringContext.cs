namespace OmniXaml
{
    using System;
    using Typing;

    public class WiringContext
    {
        public WiringContext(ITypeContext typeContext, ITypeFeatureProvider typeFeatureProvider)
        {
            FeatureProvider = typeFeatureProvider;
            TypeContext = typeContext;            
        }

        public ITypeContext TypeContext { get; }

        public ITypeFeatureProvider FeatureProvider { get; }

        public XamlType GetType(Type type)
        {
            return TypeContext.GetXamlType(type);
        }

        public XamlMember GetMember(XamlType xamlType, string name)
        {
            return xamlType.GetMember(name);
        }

        public AttachableXamlMember GetAttachableMember(XamlType xamlType, string name)
        {
            return xamlType.GetAttachableMember(name);
        }
    }
}