namespace XamlViewer
{
    public class Snippet
    {
        public Snippet(string name, string xaml)
        {
            Name = name;
            Xaml = xaml;
        }

        public string Name { get; set; }
        public string Xaml { get; set; }
    }
}