namespace OmniXaml.Tests.Classes
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Another;
    using Attributes;

    [ContentProperty("Items")]
    public class DummyClass : DummyObject
    {
        private ChildClass child;

        public DummyClass()
        {
            Items = new Collection<Item>();
            Resources = new Dictionary<string, object>();
        }

        public string SampleProperty { get; set; }
        public string AnotherProperty { get; set; }

        public ChildClass Child
        {
            get { return child; }
            set
            {
                child = value;
                if (child.Title != null)
                {
                    TitleWasSetBeforeBeingAssociated = true;
                }
            }
        }

        public Foreigner ChildFromAnotherNamespace { get; set; }
        public int Number { get; set; }
        public Collection<Item> Items { get; set; }
        public IDictionary<string, object> Resources { get; set; }
        public Item Item { get; set; }
        public SomeEnum EnumProperty { get; set; }
        public SomeEnum? NullableEnumProperty { get; set; }

        public bool TitleWasSetBeforeBeingAssociated { get; set; }
    }

    public enum SomeEnum
    {
        One,
        Two,
        Three
    }
}