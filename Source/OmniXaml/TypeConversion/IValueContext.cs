namespace OmniXaml.TypeConversion
{
    using ObjectAssembler.Commands;
    using Typing;

    public interface IValueContext
    {
        ITypeRepository TypeRepository { get; }
        ITopDownValueContext TopDownValueContext { get;}        
    }
}