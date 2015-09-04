namespace OmniXaml
{
    using Typing;

    public interface ITypeContext : IXamlNamespaceRegistry, IXamlTypeRepository
    {
        ITypeFactory TypeFactory { get; }
        IXamlTypeRepository TypeRepository { get; }
    }
}