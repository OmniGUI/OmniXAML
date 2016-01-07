namespace OmniXaml.Wpf
{
    using System;
    using Typing;

    public class WpfXamlType : XamlType
    {
        public WpfXamlType(Type type, ITypeRepository typeRepository, ITypeFactory typeFactory, ITypeFeatureProvider featureProvider)
            : base(type, typeRepository, typeFactory, featureProvider)
        {           
        }

        protected override Member LookupMember(string name)
        {
            return new WpfMember(name, this, TypeRepository, FeatureProvider);
        }

        public override string ToString()
        {
            return "WPF "+ base.ToString();
        }
    }
}