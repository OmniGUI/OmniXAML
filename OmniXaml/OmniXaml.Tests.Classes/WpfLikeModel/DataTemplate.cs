namespace OmniXaml.Tests.Classes.WpfLikeModel
{
    using Attributes;
    using Templates;

    [ContentProperty("Content")]
    public class DataTemplate
    {
        public TemplateContent Content { get; set; }
    }
}