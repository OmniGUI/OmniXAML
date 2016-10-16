namespace OmniXaml.Tests.Model
{
    internal class Window
    {
        public string Title { get; set; }
        public object Content { get; set; }

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}, {nameof(Content)}: {Content}";
        }
    }
}