namespace OmniXaml
{
    using System;

    public class AssignmentNode
    {
        public string Attr { get; set; }
        public string Value { get; set; }

        public AssignmentNode(string attr, string value)
        {
            Attr = attr;
            Value = value;
        }

        protected bool Equals(AssignmentNode other)
        {
            return String.Equals(Attr, other.Attr) && String.Equals(Value, other.Value);
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
            return Equals((AssignmentNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Attr != null ? Attr.GetHashCode() : 0)*397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}