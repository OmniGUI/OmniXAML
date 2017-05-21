using System;
using System.Linq;
using OmniXaml.Attributes;
using Zafiro.Core;

namespace OmniXaml.Tests.Model
{
    public static class Converters
    {
        [TypeConverterMember(typeof(ReferenceTarget))]
        public static Func<string, ConvertContext, (bool, object)> Convert = (o, c) =>
        {
            if (c != null)
            {
                var node = c.Node;
                var dicotomize = o.Dicotomize('.');
                var name = dicotomize.Item1;
                var prop = dicotomize.Item2;
                var target = LookupName(name, node);

                if (target != null)
                {
                    var instance = target.Instance;
                    return (true, new ReferenceTarget() { Instance = instance, PropertyName = prop });
                }                
            }

            return (false, null);
        };

        private static ConstructionNode LookupName(string name, ConstructionNode node)
        {
            var root = FindRoot(node);

            return LookupNameCore(name, root);
        }

        private static ConstructionNode LookupNameCore(string name, ConstructionNode root)
        {
            if (string.Equals(root.Name, name))
            {
                return root;
            }

            var children = root.GetAllChildren();

            return children.Select(node => LookupNameCore(name, node)).FirstOrDefault(childLookup => childLookup != null);
        }

        private static ConstructionNode FindRoot(ConstructionNode node)
        {
            if (node.Parent == null)
            {
                return node;
            }
            else
            {
                return FindRoot(node.Parent);
            }
        }
    }
}