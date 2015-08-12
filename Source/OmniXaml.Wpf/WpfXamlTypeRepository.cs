namespace OmniXaml.Wpf
{
    using System;
    using Glass;
    using Typing;

    public class WpfXamlTypeRepository : XamlTypeRepository
    {
        public WpfXamlTypeRepository(IXamlNamespaceRegistry xamlNamespaceRegistry, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
            : base(xamlNamespaceRegistry, typeTypeFactory, featureProvider)
        {
        }

        public override XamlType GetXamlType(Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));

            return new WpfXamlType(type, this, TypeFactory, FeatureProvider);
        }
    }
}