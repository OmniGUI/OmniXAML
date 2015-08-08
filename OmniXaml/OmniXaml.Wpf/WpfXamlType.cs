namespace OmniXaml.Wpf
{
    using System;
    using Typing;

    public class WpfXamlType : XamlType
    {
        public WpfXamlType(Type type, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory, ITypeFeatureProvider featureProvider)
            : base(type, xamlTypeRepository, typeFactory, featureProvider)
        {           
        }

        protected override XamlMember LookupMember(string name)
        {
            return new WpfXamlMember(name, this, TypeRepository, FeatureProvider);
        }

        public override string ToString()
        {
            return "WPF "+ base.ToString();
        }
    }
}