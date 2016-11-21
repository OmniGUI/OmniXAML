namespace OmniXaml.TypeLocation
{
    public class PrefixDeclaration
    {
        private readonly string prefix;

        private readonly string namespaceName;

        public PrefixDeclaration(string prefix, string namespaceName)
        {
            this.prefix = prefix;
            this.namespaceName = namespaceName;
        }

        public string Prefix => prefix;

        public string NamespaceName => namespaceName;

        protected bool Equals(PrefixDeclaration other)
        {
            return string.Equals(prefix, other.prefix) && string.Equals(namespaceName, other.namespaceName);
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

            return Equals((PrefixDeclaration)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (prefix.GetHashCode() * 397) ^ namespaceName.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"Prefix {Prefix} => {NamespaceName}";
        }
    }
}