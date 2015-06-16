namespace TestApplication
{
    class TestViewModel : ViewModel
    {
        public TestViewModel()
        {
            SampleString = "This came from a Binding!";
        }
        public string SampleString { get; set; }
    }
}