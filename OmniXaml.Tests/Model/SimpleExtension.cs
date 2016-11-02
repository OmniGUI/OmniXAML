namespace OmniXaml.Tests.Model
{
    public class SimpleExtension : IMarkupExtension
    {
        public object GetValue(ValueContext context)
        {
            return Property;
        }

        public string Property { get; set; }
    }
}