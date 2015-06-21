namespace OmniXaml.Wpf
{
    using System;
    using System.Text;
    using Typing;

    public class WpfXamlType : XamlType
    {
        private readonly IXamlTypeRepository typeRepository;

        public WpfXamlType(Type type, IXamlTypeRepository typeRepository) : base(type, typeRepository)
        {
            this.typeRepository = typeRepository;
        }

        protected override XamlMember LookupMember(string name)
        {
            return new WpfXamlMember(name, this, typeRepository, false);
        }

        protected override XamlMember LookupAttachableMember(string name)
        {
            return new WpfXamlMember(name, this, typeRepository, true);
        }
    }
}