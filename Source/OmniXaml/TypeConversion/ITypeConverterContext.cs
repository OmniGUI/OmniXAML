namespace OmniXaml.TypeConversion
{
    using ObjectAssembler.Commands;
    using Typing;

    public interface ITypeConverterContext
    {
        ITypeRepository TypeRepository { get; }

        ITopDownValueContext TopDownValueContext { get;}
    }
}