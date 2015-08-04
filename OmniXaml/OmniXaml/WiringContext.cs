namespace OmniXaml
{
    using System;
    using TypeConversion;
    using Typing;

    public class WiringContext
    {
        private readonly ITypeFeatureProvider typeFeatureProvider;

        public WiringContext(ITypeContext typeContext, ITypeFeatureProvider typeFeatureProvider)
        {
            this.typeFeatureProvider = typeFeatureProvider;
            TypeContext = typeContext;            
        }

        public ITypeContext TypeContext { get; }
        public ITypeConverterProvider ConverterProvider => typeFeatureProvider;

        public ITypeFeatureProvider FeatureProvider
        {
            get { return typeFeatureProvider; }
        }

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