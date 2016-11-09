namespace OmniXaml
{
    public interface IMarkupExtension
    {
        object GetValue(ExtensionValueContext context);
    }
}