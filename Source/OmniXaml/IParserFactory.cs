namespace OmniXaml
{
    using ObjectAssembler;

    public interface IParserFactory
    {
        IParser Create(Settings settings);
    }
}