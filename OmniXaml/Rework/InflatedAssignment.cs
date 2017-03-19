namespace OmniXaml.Rework
{
    using System.Collections.Generic;

    internal class InflatedAssignment
    {
        public IEnumerable<object> Instances { get; set; }
        public Member Member { get; set; }

        protected bool Equals(InflatedAssignment other)
        {
            return Member.Equals(other.Member);
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
            return Member.GetHashCode();
        }
    }
}