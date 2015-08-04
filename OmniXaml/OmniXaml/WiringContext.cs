namespace OmniXaml
{
    using System;
    using TypeConversion;
    using Typing;

    public class WiringContext
    {
        public WiringContext(ITypeContext typeContext, TypeFeatureProvider typeFeatureProvider)
        {
            TypeContext = typeContext;
            ContentPropertyProvider = typeFeatureProvider.ContentPropertyProvider;
            ConverterProvider = typeFeatureProvider.ConverterProvider;
        }

        public ITypeContext TypeContext { get; }
        public IContentPropertyProvider ContentPropertyProvider { get; private set; }
        public ITypeConverterProvider ConverterProvider { get; private set; }

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