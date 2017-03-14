namespace OmniXaml.Tests.Model
{
    public class MyImmutable : ModelObject
    {
        private readonly string argument;

        public MyImmutable(string argument)
        {
            this.Argument = argument;
        }

        public string Argument { get; }

        protected bool Equals(MyImmutable other)
        {
            return string.Equals(argument, other.argument);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((MyImmutable) obj);
        }

        public override int GetHashCode()
        {
            return (argument != null ? argument.GetHashCode() : 0);
        }
    }
}