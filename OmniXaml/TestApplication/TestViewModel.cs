namespace TestApplication
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Data;
    using System.Windows.Documents;

    class TestViewModel : ViewModel
    {
        public TestViewModel()
        {
            SampleString = "This came from a Binding!";
            Collection = new Collection<string> { "Saludos", "Cordiales" };
        }
        public string SampleString { get; set; }

        public Collection<string> Collection { get; set; }
    }
}