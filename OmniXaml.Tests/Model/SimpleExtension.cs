namespace OmniXaml.Tests.Model
{
    public class SimpleExtension : IMarkupExtension
    {
        public object GetValue()
        {
            return Property;
        }

        public string Property { get; set; }
    }
}