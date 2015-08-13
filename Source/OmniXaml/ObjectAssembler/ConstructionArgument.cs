namespace OmniXaml.ObjectAssembler
{
    public class ConstructionArgument
    {
        public string StringValue { get; }

        public ConstructionArgument(string stringValue)
        {
            StringValue = stringValue;
        }

        public ConstructionArgument(string stringValue, string value)
        {
            StringValue = stringValue;
            Value = value;
        }

        public object Value { get; set; }

        protected bool Equals(ConstructionArgument other)
        {
            return string.Equals(StringValue, other.StringValue) && Equals(Value, other.Value);
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
            return Equals((ConstructionArgument) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((StringValue != null ? StringValue.GetHashCode() : 0)*397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}