namespace OmniXaml
{
    using System;

    public interface IPrefixedTypeResolver
    {
        Type GetTypeByPrefix(ConstructionNode node, string prefixedType);
        ConstructionNode Root { get; set; }
    }
}