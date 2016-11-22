namespace OmniXaml.Tests.Model
{
    using System.Collections.Generic;
    using Attributes;

    public class ItemsControl : ModelObject
    {
        public string HeaderText { get; set; }
        public DataTemplate ItemTemplate { get; set; }
        [Content]
        public ICollection<object> Items { get; set; } = new List<object>();

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