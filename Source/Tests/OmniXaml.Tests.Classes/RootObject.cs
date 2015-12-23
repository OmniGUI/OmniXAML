namespace OmniXaml.Tests.Classes
{
    public class RootObject
    {
        public RootObject()
        {
            Collection = new CustomCollection();
        }

        public CustomCollection Collection { get; set; }
    }
}