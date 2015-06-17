namespace OmniXaml
{
    using System;
    using TypeConversion;
    using Typing;

    public class WiringContext
    {
        public WiringContext(ITypeContext typeContext, IContentPropertyProvider contentPropertyProvider, ITypeConverterProvider converterProvider)
        {
            TypeContext = typeContext;
            ContentPropertyProvider = contentPropertyProvider;
            ConverterProvider = converterProvider;
        }

        public ITypeContext TypeContext { get; private set; }
        public IContentPropertyProvider ContentPropertyProvider { get; private set; }
        public ITypeConverterProvider ConverterProvider { get; private set; }

        public XamlType GetType(Type type)
        {
            return TypeContext.Get(type);
        }

        public XamlMember GetMember(XamlType xamlType, string name)
        {
            return xamlType.GetMember(name);
        }

        public XamlMember GetAttachableMember(XamlType xamlType, string name)
        {
            return xamlType.GetAttachableMember(name);
        }
    }
}