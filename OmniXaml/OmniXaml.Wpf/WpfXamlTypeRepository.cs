namespace OmniXaml.Wpf
{
    using System;
    using Glass;
    using Typing;

    public class WpfXamlTypeRepository : XamlTypeRepository
    {
        public WpfXamlTypeRepository(IXamlNamespaceRegistry xamlNamespaceRegistry) : base(xamlNamespaceRegistry)
        {
        }

        public override XamlType GetXamlType(Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));

            return new WpfXamlType(type, this);
        }
    }
}