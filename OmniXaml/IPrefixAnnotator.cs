namespace OmniXaml
{
    using System.Collections.Generic;
    using TypeLocation;

    public interface IPrefixAnnotator
    {        
        void Annotate(ConstructionNode node, IEnumerable<PrefixDeclaration> prefixes);
        IEnumerable<PrefixDeclaration> GetPrefixesFor(ConstructionNode node);
    }
}