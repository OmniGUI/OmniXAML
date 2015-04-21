namespace OmniXaml.Parsers.MarkupExtensions
{
    public class AssignmentNode
    {
        public string Property { get; }
        public TreeNode Value { get; }

        public AssignmentNode(string property, TreeNode node)
        {
            Property = property;
            Value = node;
        }

        public override string ToString()
        {
            return $"{Property}={Value}";
        }

        protected bool Equals(AssignmentNode other)
        {
            return string.Equals(Property, other.Property) && Equals(Value, other.Value);
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
            return Equals((AssignmentNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Property != null ? Property.GetHashCode() : 0)*397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}