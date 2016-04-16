namespace OmniXaml.Wpf
{
    using System;
    using Glass.Core;
    using Typing;

    public class WpfTypeRepository : TypeRepository
    {
        public WpfTypeRepository(INamespaceRegistry namespaceRegistry, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
            : base(namespaceRegistry, typeTypeFactory, featureProvider)
        {
        }

        public override XamlType GetByType(Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));

            return new WpfXamlType(type, this, TypeFactory, FeatureProvider);
        }
    }
}