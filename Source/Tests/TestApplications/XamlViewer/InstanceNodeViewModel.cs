namespace XamlViewer
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using OmniXaml.Services.Mvvm;

    public class InstanceNodeViewModel : ViewModel
    {
        private bool isExpanded;

        public InstanceNodeViewModel(object data)
        {
            Data = data;
            IsExpanded = true;
            TypeName = data.GetType().Name;
            Tag = data.ToString();

            foreach (var runtimeProperty in data.GetType().GetRuntimeProperties())
            {
                if (typeof (ICollection).IsAssignableFrom(runtimeProperty.PropertyType))
                {
                    var collection = runtimeProperty.GetValue(data) as ICollection;

                    foreach (var item in collection)
                    {
                        Children.Add(new InstanceNodeViewModel(item));
                    }
                }
            }
        }

        public string Tag { get; set; }
        public ObservableCollection<InstanceNodeViewModel> Children { get; } = new ObservableCollection<InstanceNodeViewModel>();
        public string TypeName { get; set; }
        public object Data { get; }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                OnPropertyChanged();
            }
        }
    }
}