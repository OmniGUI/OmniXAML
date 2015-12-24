namespace OmniXaml.Tests.Classes
{
    public class RootObject
    {
        private CustomCollection collection;
        public bool CollectionWasReplaced { get; private set; }

        public RootObject()
        {
            collection = new CustomCollection();
        }

        public CustomCollection Collection
        {
            get { return collection; }
            private set
            {
                collection = value;
                CollectionWasReplaced = true;
            }
        }
    }
}