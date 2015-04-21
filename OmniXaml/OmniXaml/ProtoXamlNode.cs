namespace OmniXaml
{
    using Parsers.ProtoParser;
    using Typing;

    public class ProtoXamlNode
    {
        public XamlType XamlType { get; set; }
        public string Namespace { get; set; }
        public ProtoNodeType NodeType { get; set; }
        public XamlMember PropertyAttribute { get; set; }
        public string Prefix { get; set; }

        protected bool Equals(ProtoXamlNode other)
        {
            return XamlTypesAreEqual(other) && string.Equals(Namespace, other.Namespace) && NodeType == other.NodeType;
        }

        private bool XamlTypesAreEqual(ProtoXamlNode other)
        {
            return XamlType.UnderlyingType == other.XamlType.UnderlyingType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ProtoXamlNode)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = XamlType?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Namespace?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (int)NodeType;
                return hashCode;
            }
        }
    }
}