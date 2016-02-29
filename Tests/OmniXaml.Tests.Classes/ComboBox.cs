namespace OmniXaml.Tests.Classes
{
    using System.Collections.ObjectModel;
    using Attributes;

    public class ComboBox : Selector
    {      
    }

    [ContentProperty("Items")]
    public class ItemsControl : DummyObject
    {
        public Collection<object> Items { get; set; } = new Collection<object>();
    }

    public class ListBoxItem : ContentControl
    {
    }

    public class BindingExtension : IMarkupExtension
    {
        public BindingExtension(string path)
        {
            Path = path;
        }

        public object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            return null;
        }

        public BindingMode Mode { get; set; }
        public string Path { get; set; }
    }

    public enum BindingMode
    {
        Default,
        OneWay,
        TwoWay,
        OneWayToSource,
    }
}