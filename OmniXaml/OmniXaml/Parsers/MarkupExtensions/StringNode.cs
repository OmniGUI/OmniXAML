namespace OmniXaml.Parsers.MarkupExtensions
{
    internal class StringNode : TreeNode
    {
        private readonly string str;

        public StringNode(string str)
        {
            this.str = str;
        }

        protected bool Equals(StringNode other)
        {
            return string.Equals(str, other.str);
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
            return Equals((StringNode) obj);
        }

        public override int GetHashCode()
        {
            return (str != null ? str.GetHashCode() : 0);
        }
    }
}