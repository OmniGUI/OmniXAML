namespace OmniXaml.Metadata
{
    using System.Collections.Generic;

    public class DependencyRegistrations : HashSet<DependencyRegistration>
    {
        public DependencyRegistrations()
        {
        }

        public DependencyRegistrations(IEnumerable<DependencyRegistration> collection)
            : base(collection)
        {
        }

        public override bool Equals(object obj)
        {
            return SetEquals((IEnumerable<DependencyRegistration>) obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}