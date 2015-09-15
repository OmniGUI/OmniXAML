namespace OmniXaml.TypeConversion
{
    using ObjectAssembler.Commands;
    using Typing;

    public interface IXamlTypeConverterContext
    {
        IXamlTypeRepository TypeRepository { get; }

        ITopDownValueContext TopDownValueContext { get;}
    }
}