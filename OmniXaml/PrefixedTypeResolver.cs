namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Zafiro.Core;
    using TypeLocation;

    public class PrefixedTypeResolver : IPrefixedTypeResolver
    {
        private readonly IPrefixAnnotator annotator;
        private readonly ITypeDirectory typeDirectory;

        public PrefixedTypeResolver(IPrefixAnnotator annotator, ITypeDirectory typeDirectory)
        {
            this.annotator = annotator;
            this.typeDirectory = typeDirectory;
        }

        public Type GetTypeByPrefix(ConstructionNode node, string prefixedType)
        {
            var p = prefixedType.Dicotomize(':');
            var prefix = p.Item2 == null ? "": p.Item1;
            var typeName = p.Item2 ?? p.Item1;

            var availablePrefixes = GetAvailableFrom(Root, node);

            var prefixRegistration = availablePrefixes.First(registration => registration.Prefix == prefix);
            return typeDirectory.GetTypeByFullAddress(new Address(prefixRegistration.NamespaceName, typeName));
        }

        private IEnumerable<PrefixDeclaration> GetAvailableFrom(ConstructionNode root, ConstructionNode lastToCheck)
        {
            var myPrefixes = annotator.GetPrefixesFor(root);

            IEnumerable<PrefixDeclaration> prefixesFromChildren;
            if (root != lastToCheck)
            {
                prefixesFromChildren = root.Assignments
                    .SelectMany(assignment => assignment.Values)
                    .Select(constructionNode => GetAvailableFrom(constructionNode, lastToCheck))
                    .SelectMany(enumerable => enumerable);
            }
            else
            {
                prefixesFromChildren = new PrefixDeclaration[0];
            }
            
            return myPrefixes.Concat(prefixesFromChildren).ToList();
        }

        public ConstructionNode Root { get; set; }
    }
}