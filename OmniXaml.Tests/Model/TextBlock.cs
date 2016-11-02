namespace OmniXaml.Tests.Model
{
    using DefaultLoader;

    public class TextBlock : ModelObject
    {
        [Content]
        public string Text { get; set; }

        protected bool Equals(TextBlock other)
        {
            return string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((TextBlock) obj);
        }

        public override int GetHashCode()
        {
            return (Text != null ? Text.GetHashCode() : 0);
        }
    }
}