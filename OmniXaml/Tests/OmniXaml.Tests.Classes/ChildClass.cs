using OmniXaml.Tests.Classes;

namespace OmniXaml.Tests.Classes
{
    using Attributes;

    [ContentProperty("Content")]
    public class ChildClass
    {
        public ChildClass Child { get; set; }
        public Item Content { get; set; }
        public string Name { get; set; }
    }
}