namespace OmniXaml.Tests.Classes
{
    using Attributes;

    [ContentProperty("Content")]
    public class ContentControl : DummyObject
    {
        public object Content { get; set; }        
    }
}