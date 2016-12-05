namespace OmniXaml.Tests.Model.Custom
{
    internal class CustomWindow : Window
    {
        public string CustomProperty { get; set; }

        protected bool Equals(CustomWindow other)
        {
            return base.Equals(other) && string.Equals(CustomProperty, other.CustomProperty);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CustomWindow) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (CustomProperty != null ? CustomProperty.GetHashCode() : 0);
            }
        }
    }
}