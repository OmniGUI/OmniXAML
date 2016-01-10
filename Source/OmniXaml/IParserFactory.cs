namespace OmniXaml
{
    using ObjectAssembler;

    public interface IParserFactory
    {
        IParser CreateForReadingFree();
        IParser CreateForReadingSpecificInstance(object rootInstance);
        IParser Create(ObjectAssemblerSettings settings);
    }
}