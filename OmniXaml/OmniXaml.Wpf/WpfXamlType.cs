namespace OmniXaml.Wpf
{
    using System;
    using Typing;

    public class WpfXamlType : XamlType
    {
        public WpfXamlType(Type type, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
            : base(type, xamlTypeRepository, typeTypeFactory, featureProvider)
        {           
        }

        protected override XamlMember LookupMember(string name)
        {
            return new WpfXamlMember(name, this, TypeRepository, TypeFactory, FeatureProvider);
        }

        protected override AttachableXamlMember LookupAttachableMember(string name)
        {
            return new WpfXamlAttachableMember(name, this, TypeRepository, TypeFactory, FeatureProvider);
        }
    }
}