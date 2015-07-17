namespace OmniXaml.Wpf
{
    using System;
    using Glass;
    using Typing;

    public class WpfXamlTypeRepository : XamlTypeRepository
    {
        private readonly ITypeFactory typeFactory;

        public WpfXamlTypeRepository(IXamlNamespaceRegistry xamlNamespaceRegistry, ITypeFactory typeFactory) : base(xamlNamespaceRegistry, typeFactory)
        {
            this.typeFactory = typeFactory;
        }

        public override XamlType GetXamlType(Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));

            return new WpfXamlType(type, this, typeFactory);
        }
    }
}