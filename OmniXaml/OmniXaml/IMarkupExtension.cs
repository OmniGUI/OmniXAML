namespace OmniXaml
{
    public interface IMarkupExtension
    {
        object ProvideValue(XamlToObjectWiringContext extensionContext);
    }
}