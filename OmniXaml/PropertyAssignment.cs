namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;

    public class PropertyAssignment
    {
        public Property Property { get; set; }
        public string SourceValue { get; set; }
        public IEnumerable<ConstructionNode> Children { get; set; } = new List<ConstructionNode>();

        public override string ToString()
        {
            if (SourceValue != null)
            {
                return $@"{Property} = ""{SourceValue}""";
            }
            else
            {
                var formattedChildren = string.Join(", ", Children);
                return $"{Property} = {formattedChildren}";
            }
        }

        protected bool Equals(PropertyAssignment other)
        {
            return Equals(Property, other.Property) && string.Equals(SourceValue, other.SourceValue) && Enumerable.SequenceEqual(Children, other.Children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((PropertyAssignment) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Property != null ? Property.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SourceValue != null ? SourceValue.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Children != null ? Children.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}