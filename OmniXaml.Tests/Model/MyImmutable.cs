namespace OmniXaml.Tests.Model
{
    public class MyImmutable : ModelObject
    {
        private readonly string arg;

        public MyImmutable(string arg)
        {
            this.arg = arg;
        }

        protected bool Equals(MyImmutable other)
        {
            return string.Equals(arg, other.arg);
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
            return (arg != null ? arg.GetHashCode() : 0);
        }
    }
}