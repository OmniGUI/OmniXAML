namespace OmniXaml.Ambient
{
    using System.Reflection;

    public class AmbientMemberAssignment
    {
        public Member Property { get; set; }
        public object Value { get; set; }

        protected bool Equals(AmbientMemberAssignment other)
        {
            return Equals(Property, other.Property) && Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((AmbientMemberAssignment) obj);
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