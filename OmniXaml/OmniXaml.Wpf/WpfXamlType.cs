namespace OmniXaml.Wpf
{
    using System;
    using System.Text;
    using Typing;

    public class WpfXamlType : XamlType
    {
        private readonly IXamlTypeRepository wiringContext;

        public WpfXamlType(Type type, IXamlTypeRepository wiringContext) : base(type, wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        protected override XamlMember LookupMember(string name)
        {
            return new WpfXamlMember(name, this, wiringContext, false);
        }

        protected override XamlMember LookupAttachableMember(string name)
        {
            return new WpfXamlMember(name, this, wiringContext, true);
        }
    }
}