namespace OmniXaml.TypeLocation
{
    public class Address
    {
        public string Namespace { get; set; }
        public string TypeName { get; set; }

        public override string ToString()
        {
            return $"{nameof(Namespace)}: {Namespace}, {nameof(TypeName)}: {TypeName}";
        }
    }
}