namespace OmniXaml.Tests.Model
{
    public class SimpleExtension : IMarkupExtension
    {
        public object GetValue(MarkupExtensionContext context)
        {
            return Property;
        }

        public string Property { get; set; }
    }
}