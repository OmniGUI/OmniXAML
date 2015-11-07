namespace OmniXaml.Tests.Classes.WpfLikeModel
{
    using Attributes;

    [ContentProperty("Text")]
    public class TextBlock : FrameworkElement
    {        
        public string Text { get; set; }
    }
}