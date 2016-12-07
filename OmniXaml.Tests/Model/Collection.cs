namespace OmniXaml.Tests.Model
{
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Collection : Collection<object>
    {
        public string Title { get; set; }

        protected bool Equals(Collection other)
        {
            return string.Equals(Title, other.Title) && Enumerable.SequenceEqual(this, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Collection) obj);
        }

        public override int GetHashCode()
        {
            return (Title != null ? Title.GetHashCode() : 0);
        }
    }
}