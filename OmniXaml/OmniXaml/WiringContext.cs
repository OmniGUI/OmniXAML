namespace OmniXaml
{
    using System;
    using System.Reflection;
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

        public virtual XamlType GetType(Type type)
        {
            return TypeContext.GetXamlType(type);
        }

        public virtual AttachableXamlMember GetAttachableMember(string name, MethodInfo getter, MethodInfo setter, IXamlTypeRepository xamlTypeRepository, ITypeFeatureProvider featureProvider)
        {
            return new AttachableXamlMember(name, getter, setter, xamlTypeRepository, featureProvider);
        }
    }
}