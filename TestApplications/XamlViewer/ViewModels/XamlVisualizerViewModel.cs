namespace XamlViewer.ViewModels
{
    using OmniXaml;
    using OmniXaml.Services.Mvvm;

    public class XamlVisualizerViewModel : ViewModel
    {
        private string xaml;

        public XamlVisualizerViewModel()
        {
            Xaml = string.Empty;
        }

        public IRuntimeTypeSource RuntimeTypeSource { get; protected set; }

        public string Xaml
        {
            get { return xaml; }
            set
            {
                xaml = value;
                OnPropertyChanged();
            }
        }
    }
}