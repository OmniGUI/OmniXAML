namespace OmniXaml
{
    using Assembler;

    public interface IObjectAssemblerFactory
    {
        IObjectAssembler GetAssembler(ObjectAssemblerSettings settings);
    }
}