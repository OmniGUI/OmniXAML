namespace OmniXaml
{
    using Assembler;

    public interface IObjectAssemblerFactory
    {
        IObjectAssembler CreateAssembler(ObjectAssemblerSettings settings);
        IObjectAssembler CreateAssembler();
    }
}