namespace OmniXaml.Typing
{
    using System;
    using System.Reflection;

    public class ClrNamespace : Namespace
    {
        private readonly Assembly assembly;
        private readonly string ns;

        public ClrNamespace(Assembly assembly, string ns)
        {
            this.assembly = assembly;
            this.ns = ns;
            this.Name = ns;
        }

        public Assembly Assembly => assembly;

        public string Namespace => ns;

        public override Type Get(string typeName)
        {
            return assembly.GetType(ns + "." + typeName);
        }

        protected bool Equals(ClrNamespace other)
        {
            return assembly.Equals(other.assembly) && string.Equals(ns, other.ns);
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
            return Equals((ClrNamespace) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (assembly.GetHashCode()*397) ^ ns.GetHashCode();
            }
        }
    }
}