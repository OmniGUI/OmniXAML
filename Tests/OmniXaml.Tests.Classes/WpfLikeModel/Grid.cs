namespace OmniXaml.Tests.Classes.WpfLikeModel
{
    using System.Collections.ObjectModel;
    using Attributes;

    [ContentProperty("Children")]
    public class Grid
    {
        public Collection<RowDefinition> RowDefinitions { get; set; }
        public Collection<object> Children { get; set; } = new Collection<object>();
    }
}