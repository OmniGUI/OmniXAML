namespace OmniXaml
{
    public interface IMarkupExtension
    {
        object GetValue(ValueContext context);
    }
}