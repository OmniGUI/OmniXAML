namespace OmniXaml.TypeConversion
{
    using Typing;

    public interface IXamlTypeConverterContext
    {
        IXamlTypeRepository TypeRepository { get; }
    }
}