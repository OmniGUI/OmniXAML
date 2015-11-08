namespace OmniXaml.Typing
{
    public struct DependencyRegistration
    {
        public string Property { get; set; }
        public string DependsOn { get; set; }

        public DependencyRegistration(string property, string dependsOn)
        {
            Property = property;
            DependsOn = dependsOn;
        }

        public bool Equals(DependencyRegistration other)
        {
            return string.Equals(Property, other.Property) && string.Equals(DependsOn, other.DependsOn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is DependencyRegistration && Equals((DependencyRegistration) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Property != null ? Property.GetHashCode() : 0)*397) ^ (DependsOn != null ? DependsOn.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return Property + " depends on " + DependsOn;
        }
    }
}