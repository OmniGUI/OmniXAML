namespace OmniXaml.Wpf
{
    using System;
    using Glass;
    using Typing;

    public class XamlTypeRepository : Typing.XamlTypeRepository
    {
        private readonly ITypeFactory typeFactory;

        public XamlTypeRepository(IXamlNamespaceRegistry xamlNamespaceRegistry, ITypeFactory typeFactory) : base(xamlNamespaceRegistry, typeFactory)
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