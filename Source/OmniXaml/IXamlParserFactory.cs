namespace OmniXaml
{
    public interface IXamlParserFactory
    {
        IXamlParser CreateForReadingFree();
        IXamlParser CreateForReadingSpecificInstance(object rootInstance);
    }
}