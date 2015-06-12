namespace OmniXaml.Typing
{
    public class PrefixRegistration
    {
        private readonly string prefix;

        private readonly string ns;

        public PrefixRegistration(string prefix, string ns)
        {
            this.prefix = prefix;
            this.ns = ns;
        }

        public string Prefix => prefix;

        public string Ns => ns;

        protected bool Equals(PrefixRegistration other)
        {
            return string.Equals(prefix, other.prefix) && string.Equals(ns, other.ns);
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

            return Equals((PrefixRegistration) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (prefix.GetHashCode()*397) ^ ns.GetHashCode();
            }
        }
    }
}