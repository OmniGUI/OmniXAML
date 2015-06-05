namespace OmniXaml
{
    using Catalogs;
    using Typing;

    public interface ITypeContext : IXamlNamespaceRegistry, IXamlTypeRepository
    {
        ITypeFactory TypeFactory { get; }        
    }
}