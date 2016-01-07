namespace OmniXaml.Tests.Classes
{
    using System.Collections.ObjectModel;
    using Attributes;
    using Templates;

    [ContentProperty("Children")]
    public class Item
    {
        public Item()
        {
            Children = new Collection<Item>();
        }

        public string Title { get; set; }
        public string Text { get; set; }
        public Collection<Item> Children { get; set; }
        public Template Template { get; set; }
    }
}