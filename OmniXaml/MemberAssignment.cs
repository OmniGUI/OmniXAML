namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;

    public class MemberAssignment
    {
        public Member Member { get; set; }
        public string SourceValue { get; set; }
        public IEnumerable<ConstructionNode> Children { get; set; } = new List<ConstructionNode>();

        public override string ToString()
        {
            if (SourceValue != null)
            {
                return $@"{Member} = ""{SourceValue}""";
            }
            else
            {
                var formattedChildren = string.Join(", ", Children);
                return $"{Member} = {formattedChildren}";
            }
        }

        protected bool Equals(MemberAssignment other)
        {
            return Equals(Member, other.Member) && string.Equals(SourceValue, other.SourceValue) && Enumerable.SequenceEqual(Children, other.Children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((MemberAssignment) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Member != null ? Member.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SourceValue != null ? SourceValue.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Children != null ? Children.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}