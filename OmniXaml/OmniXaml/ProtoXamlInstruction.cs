namespace OmniXaml
{
    using Parsers.ProtoParser;
    using Typing;

    public class ProtoXamlInstruction
    {
        public XamlType XamlType { get; set; }
        public string Namespace { get; set; }
        public NodeType NodeType { get; set; }
        public XamlMemberBase PropertyAttribute { get; set; }
        public string Prefix { get; set; }
        public XamlMember PropertyElement { get; set; }
        public string PropertyAttributeText { get; set; }
        public string Text { get; set; }

        protected bool Equals(ProtoXamlInstruction other)
        {
            return Equals(XamlType, other.XamlType) && string.Equals(Namespace, other.Namespace) && NodeType == other.NodeType &&
                   Equals(PropertyAttribute, other.PropertyAttribute) && string.Equals(Prefix, other.Prefix) && Equals(PropertyElement, other.PropertyElement) &&
                   string.Equals(PropertyAttributeText, other.PropertyAttributeText) && string.Equals(Text, other.Text);
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
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((ProtoXamlInstruction)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (XamlType != null ? XamlType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Namespace != null ? Namespace.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)NodeType;
                hashCode = (hashCode * 397) ^ (PropertyAttribute != null ? PropertyAttribute.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Prefix != null ? Prefix.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PropertyElement != null ? PropertyElement.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PropertyAttributeText != null ? PropertyAttributeText.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Text != null ? Text.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            object[] nodeType = { NodeType };

            var str = string.Format("{0}: ", nodeType);
            switch (NodeType)
            {

                case NodeType.EmptyElement:
                case NodeType.Element:
                    {
                        str = string.Concat(str, XamlType.Name);
                        return str;
                    }
                case NodeType.Attribute:
                    str = string.Concat(str, PropertyAttributeText);
                    return str;
                case NodeType.PropertyElement:
                case NodeType.EmptyPropertyElement:
                    str = string.Concat(str, PropertyElement);
                    return str;
                case NodeType.Directive:
                    str = string.Concat(str, PropertyAttribute);
                    return str;
                case NodeType.PrefixDefinition:
                    str = string.Concat($"{Prefix}:{Namespace}");
                    return str;
                default:
                    {
                        return str;
                    }
            }
        }
    }
}