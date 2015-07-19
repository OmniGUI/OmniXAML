namespace TestApplication.ViewModels
{
    using OmniXaml;

    public class XamlVisualizerViewModel : ViewModel
    {
        private string xaml;

        public XamlVisualizerViewModel()
        {
            Xaml = string.Empty;
        }

        public WiringContext WiringContext { get; protected set; }

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