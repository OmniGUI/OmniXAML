namespace OmniXaml
{
    public interface IMarkupExtension
    {
        object ProvideValue(MarkupExtensionContext markupExtensionContext);
    }
}