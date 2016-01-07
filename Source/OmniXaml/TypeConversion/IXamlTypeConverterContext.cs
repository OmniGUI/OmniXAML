namespace OmniXaml.TypeConversion
{
    using ObjectAssembler.Commands;
    using Typing;

    public interface IXamlTypeConverterContext
    {
        ITypeRepository TypeRepository { get; }

        ITopDownValueContext TopDownValueContext { get;}
    }
}