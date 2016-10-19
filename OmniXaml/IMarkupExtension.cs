namespace OmniXaml
{
    public interface IMarkupExtension
    {
        object GetValue(MarkupExtensionContext context);
    }
}