namespace XamlViewer
{
    using System.Collections.ObjectModel;
    using OmniXaml.Services.Mvvm;

    class TestViewModel : ViewModel
    {
        public TestViewModel()
        {
            SampleString = "This came from a Binding!";
            Collection = new Collection<string> {"This list", "is also", "loaded", "using a", "Binding", "It's", "absolutely", "AMAZING!!!"};
        }
        public string SampleString { get; set; }

        public Collection<string> Collection { get; set; }
    }
}