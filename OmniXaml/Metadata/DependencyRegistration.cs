namespace OmniXaml.Metadata
{
    public struct DependencyRegistration
    {
        public string PropertyName { get; set; }
        public string DependsOn { get; set; }

        public DependencyRegistration(string propertyName, string dependsOn)
        {
            PropertyName = propertyName;
            DependsOn = dependsOn;
        }

        public bool Equals(DependencyRegistration other)
        {
            return string.Equals(PropertyName, other.PropertyName) && string.Equals(DependsOn, other.DependsOn);
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
                return ((PropertyName != null ? PropertyName.GetHashCode() : 0)*397) ^ (DependsOn != null ? DependsOn.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return PropertyName + " depends on " + DependsOn;
        }
    }
}