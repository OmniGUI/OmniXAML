namespace OmniXaml
{
    internal class InflatedAssignment
    {
        public object Instance { get; set; }
        public Member Assignment { get; set; }

        protected bool Equals(InflatedAssignment other)
        {
            return Instance.Equals(other.Instance) && Assignment.Equals(other.Assignment);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InflatedAssignment) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Instance.GetHashCode() * 397) ^ Assignment.GetHashCode();
            }
        }
    }
}