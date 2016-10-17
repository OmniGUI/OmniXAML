namespace OmniXaml.Tests.Model
{
    public class ItemsControl
    {
        public DataTemplate ItemTemplate { get; set; }

        protected bool Equals(ItemsControl other)
        {
            return Equals(ItemTemplate, other.ItemTemplate);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((ItemsControl) obj);
        }

        public override int GetHashCode()
        {
            return (ItemTemplate != null ? ItemTemplate.GetHashCode() : 0);
        }
    }
}