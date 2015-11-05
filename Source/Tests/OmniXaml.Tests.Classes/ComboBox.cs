namespace OmniXaml.Tests.Classes
{
    using System.Collections.ObjectModel;

    public class ComboBox : Selector
    {      
    }

    public class Selector : ItemsControl
    {
        public int SelectedIndex { get; set; }
    }

    public class ItemsControl
    {
        public Collection<object> Items { get; set; }
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