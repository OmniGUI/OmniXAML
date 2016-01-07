namespace OmniXaml
{
    public interface IParserFactory
    {
        IParser CreateForReadingFree();
        IParser CreateForReadingSpecificInstance(object rootInstance);
    }
}