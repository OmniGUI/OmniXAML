namespace OmniXaml.TypeLocation
{
    using System;
    using System.Reflection;
    using Glass.Core;

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
            return assembly.Equals(other.assembly) && String.Equals(ns, other.ns);
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
            return Equals((ClrNamespace)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (assembly.GetHashCode() * 397) ^ ns.GetHashCode();
            }
        }

        public static ClrNamespace ExtractNamespace(string formattedClrString)
        {
            var startOfNamespace = formattedClrString.IndexOf(":", StringComparison.Ordinal) + 1;
            var endOfNamespace = formattedClrString.IndexOf(";", startOfNamespace, StringComparison.Ordinal);

            if (endOfNamespace < 0)
                endOfNamespace = formattedClrString.Length - startOfNamespace;

            var ns = formattedClrString.Substring(startOfNamespace, endOfNamespace - startOfNamespace);

            var remainingPartStart = startOfNamespace + ns.Length + 1;
            var remainingPartLenght = formattedClrString.Length - remainingPartStart;
            var assemblyPart = formattedClrString.Substring(remainingPartStart, remainingPartLenght);

            var assembly = GetAssembly(assemblyPart);

            return new ClrNamespace(assembly, ns);
        }

        public static Assembly GetAssembly(string assemblyPart)
        {
            var dicotomize = assemblyPart.Dicotomize('=');
            return Assembly.Load(new AssemblyName(dicotomize.Item2));
        }
    }
}