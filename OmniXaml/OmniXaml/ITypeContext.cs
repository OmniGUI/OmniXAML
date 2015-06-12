namespace OmniXaml
{
    using System.Collections.Generic;
    using Catalogs;
    using Typing;

    public interface ITypeContext : IXamlNamespaceRegistry, IXamlTypeRepository
    {
        ITypeFactory TypeFactory { get; }
        IEnumerable<PrefixRegistration> RegisteredPrefixes { get; }
    }
}