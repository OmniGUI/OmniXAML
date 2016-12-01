namespace OmniXaml.Tests.Model
{
    using Attributes;

    public class TextBlock : ModelObject
    {
        [Content]
        public string Text { get; set; }

        public TextWrapping TextWrapping { get; set; }


        protected bool Equals(TextBlock other)
        {
            return base.Equals(other) && string.Equals(Text, other.Text) && TextWrapping == other.TextWrapping;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((TextBlock)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Text != null ? Text.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)TextWrapping;
                return hashCode;
            }
        }
    }
}