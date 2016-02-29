namespace OmniXaml.Tests.Classes
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    public class CustomCollection : ObservableCollection<object>
    {
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }
    }
}