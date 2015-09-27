namespace OmniXaml.Tests.Classes
{
    using System.Collections.ObjectModel;

    public class ComboBox
    {
        public Collection<object> Items { get; set; }

        public int SelectedIndex { get; set; }
    }
}