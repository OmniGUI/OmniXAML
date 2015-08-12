namespace OmniXaml.Parsers.MarkupExtensions
{
    public class StringNode : TreeNode
    {
        public string Value { get; }

        public StringNode(string value)
        {
            Value = value;
        }

        protected bool Equals(StringNode other)
        {
            return string.Equals(Value, other.Value);
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
            return Equals((StringNode) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}