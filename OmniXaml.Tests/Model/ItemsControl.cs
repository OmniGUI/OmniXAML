namespace OmniXaml.Tests.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using Attributes;

    public class ItemsControl : ModelObject
    {
        public string HeaderText { get; set; }
        public DataTemplate ItemTemplate { get; set; }
        [Content]
        public ICollection<object> Items { get; set; } = new List<object>();

        protected bool Equals(ItemsControl other)
        {
            return Items != null && (base.Equals(other) && string.Equals(HeaderText, other.HeaderText) && Equals(ItemTemplate, other.ItemTemplate) && Enumerable.SequenceEqual(Items, other.Items));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ItemsControl) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (HeaderText != null ? HeaderText.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ItemTemplate != null ? ItemTemplate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Items != null ? Items.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}