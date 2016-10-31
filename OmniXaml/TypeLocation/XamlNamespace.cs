namespace OmniXaml.TypeLocation
{
    using System;

    public class XamlNamespace : Namespace
    {
        private readonly AddressPack addressPack;

        public XamlNamespace(string name)
        {
            this.Name = name;
            this.addressPack = new AddressPack();
        }

        public XamlNamespace(string name, AddressPack addressPack)
        {
            this.Name = name;
            this.addressPack = addressPack;
        }

        public AddressPack Addresses => addressPack;

        public static AssemblyNameConfig Map(string root)
        {
            return new AssemblyNameConfig(root);
        }

        public override Type Get(string typeName)
        {
            return Addresses.Get(typeName);
        }
    }
}