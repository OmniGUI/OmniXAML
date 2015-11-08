namespace OmniXaml.Typing
{
    using System.Collections.Generic;

    public class DependencyRegistrations : HashSet<DependencyRegistration>
    {
        public override bool Equals(object obj)
        {
            return SetEquals((IEnumerable<DependencyRegistration>) obj);
        }
    }
}