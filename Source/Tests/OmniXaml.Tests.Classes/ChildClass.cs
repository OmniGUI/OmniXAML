namespace OmniXaml.Tests.Classes
{
    using Attributes;

    [ContentProperty("Content")]
    public class ChildClass : DummyObject
    {
        public ChildClass Child { get; set; }
        public Item Content { get; set; }
        public string Title { get; set; }
    }
}