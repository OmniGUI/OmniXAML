namespace OmniXaml
{
    public class NamespaceDeclaration
    {
        private string ns;
        private string prefix;

        public NamespaceDeclaration(string ns, string prefix)
        {
            this.ns = ns;
            this.prefix = prefix;
        }

        public string Namespace
        {
            get { return ns; }
            set { ns = value; }
        }

        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }

        protected bool Equals(NamespaceDeclaration other)
        {
            return string.Equals(ns, other.ns) && string.Equals(prefix, other.prefix);
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
            return Equals((NamespaceDeclaration) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ns != null ? ns.GetHashCode() : 0)*397) ^ (prefix != null ? prefix.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            string finalPrefix;
            if (Prefix == null)
            {
                finalPrefix = "{No namespace}";
            } else if (Prefix == "")
            {
                finalPrefix = "{Default}";
            }
            else
            {
                finalPrefix = Prefix;
            }

            return $"{finalPrefix} => {Namespace}";
        }
    }
}