namespace OmniXaml
{
    using Typing;

    public interface ITypeContext : IXamlNamespaceRegistry, IXamlTypeRepository
    {
        IXamlTypeRepository TypeRepository { get; }
    }
}