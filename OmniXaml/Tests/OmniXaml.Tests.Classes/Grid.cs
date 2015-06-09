namespace OmniXaml.Tests.Classes
{
    using System.Collections.ObjectModel;
    using Attributes;

    [ContentProperty("Children")]
    public class Grid
    {
        public Collection<RowDefinition> RowDefinitions { get; set; }
        public Collection<TextBlock> Children { get; set; }
    }
}