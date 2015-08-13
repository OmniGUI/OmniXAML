namespace XamlViewer.ViewModels
{
    using OmniXaml;
    using OmniXaml.AppServices.Mvvm;

    public class XamlVisualizerViewModel : ViewModel
    {
        private string xaml;

        public XamlVisualizerViewModel()
        {
            Xaml = string.Empty;
        }

        public IWiringContext IWiringContext { get; protected set; }

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