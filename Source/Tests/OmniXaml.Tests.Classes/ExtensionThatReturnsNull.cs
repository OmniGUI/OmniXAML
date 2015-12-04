namespace OmniXaml.Tests.Classes
{
    public class ExtensionThatReturnsNull : IMarkupExtension
    {
        public object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            return null;
        }
    }
}