namespace OmniXaml.Typing
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Metadata
    {
        public static bool IsEmpty(Metadata metadata)
        {
            return typeof(Metadata).GetRuntimeProperties().All(info => info.GetValue(metadata) == null);
        }

        public DependencyRegistrations PropertyDependencies { get; set; }

        public void SetMemberDependency(string property, string dependsOn)
        {
            PropertyDependencies.Add(new DependencyRegistration(property, dependsOn));
        }

        public IEnumerable<string> GetMemberDependencies(string name)
        {
            if (PropertyDependencies == null)
            {
                return new List<string>();
            }

            return from dependency in PropertyDependencies
                   where dependency.Property == name
                   select dependency.DependsOn;
        }

        public string RuntimePropertyName { get; set; }

        public string ContentProperty { get; set; }


        protected bool Equals(Metadata other)
        {
            return Equals(PropertyDependencies, other.PropertyDependencies) && string.Equals(RuntimePropertyName, other.RuntimePropertyName);
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
            return Equals((Metadata)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((PropertyDependencies != null ? PropertyDependencies.GetHashCode() : 0) * 397) ^ (RuntimePropertyName != null ? RuntimePropertyName.GetHashCode() : 0);
            }
        }
    }
}