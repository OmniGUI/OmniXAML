namespace OmniXaml.Tests.Classes
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Another;
    using Attributes;

    [ContentProperty("Items")]
    public class DummyClass : DummyObject
    {
        public DummyClass()
        {
            Items = new Collection<Item>();
            Resources = new Dictionary<string, object>();
        }

        public string SampleProperty { get; set; }
        public string AnotherProperty { get; set; }
        public ChildClass Child { get; set; }
        public Foreigner ChildFromAnotherNamespace { get; set; }
        public int Number { get; set; }
        public Collection<Item> Items { get; set; }
        public IDictionary<string, object> Resources { get; set; }
    }
}