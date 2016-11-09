namespace OmniXaml.Tests.Model.Custom
{
    public class ParametrizedExtension : IMarkupExtension
    {
        public ParametrizedExtension(string str)
        {
            String = str;
        }

        public object GetValue(ExtensionValueContext context)
        {
            return String;
        }

        public string String { get; set; }

        protected bool Equals(ParametrizedExtension other)
        {
            return string.Equals(String, other.String);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((ParametrizedExtension) obj);
        }

        public override int GetHashCode()
        {
            return (String != null ? String.GetHashCode() : 0);
        }
    }
}