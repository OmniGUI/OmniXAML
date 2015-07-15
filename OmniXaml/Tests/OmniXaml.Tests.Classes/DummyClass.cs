namespace OmniXaml.Tests.Classes
{
    using System.Collections.ObjectModel;
    using Another;
    using Attributes;

    [ContentProperty("Items")]
    public class DummyClass
    {
        public DummyClass()
        {
            Items = new Collection<Item>();
        }

        public string SampleProperty { get; set; }
        public string AnotherProperty { get; set; }
        public ChildClass Child { get; set; }
        public Foreigner ChildFromAnotherNamespace { get; set; }
        public int Number { get; set; }
        public Collection<Item> Items { get; set; }
    }

    public class DummyChild : DummyClass
    {
    }
}