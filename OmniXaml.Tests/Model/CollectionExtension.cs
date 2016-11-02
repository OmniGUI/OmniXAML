namespace OmniXaml.Tests.Model
{
    using System.Collections.ObjectModel;

    public class CollectionExtension : IMarkupExtension
    {
        public object GetValue(ValueContext context)
        {
            return new Collection<object> {"Item 1", "Item 2"};
        }
    }
}