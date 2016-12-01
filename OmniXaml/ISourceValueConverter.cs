namespace OmniXaml
{
    public interface ISourceValueConverter
    {
        object GetCompatibleValue(ConverterValueContext valueContext);
    }
}