namespace OmniXaml
{
    using System.Collections.Generic;
    using TypeLocation;

    public class PrefixAnnotator : IPrefixAnnotator
    {
        readonly IDictionary<ConstructionNode, IEnumerable<PrefixDeclaration>> mappedPrefixes = new Dictionary<ConstructionNode, IEnumerable<PrefixDeclaration>>();

        public void Annotate(ConstructionNode node, IEnumerable<PrefixDeclaration> prefixes)
        {
            mappedPrefixes.Add(node, prefixes);
        }

        public IEnumerable<PrefixDeclaration> GetPrefixesFor(ConstructionNode node)
        {
            IEnumerable<PrefixDeclaration> prefixes;
            return mappedPrefixes.TryGetValue(node, out prefixes) ? prefixes : new List<PrefixDeclaration>();
        }
    }
}