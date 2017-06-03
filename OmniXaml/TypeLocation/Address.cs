namespace OmniXaml.TypeLocation
{
    public class Address
    {
        public Address(string ns, string typeName)
        {
            Namespace = ns;
            TypeName = typeName;
        }
        public string Namespace { get; set; }
        public string TypeName { get; set; }

        public override string ToString()
        {
            return $"{nameof(Namespace)}: {Namespace}, {nameof(TypeName)}: {TypeName}";
        }
    }
}