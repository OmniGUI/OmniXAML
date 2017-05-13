using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OmniXaml;
using OmniXaml.Attributes;
using OmniXaml.ReworkPhases;
using Zafiro.Core;

namespace XamlLoadTest
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
                var instance = target.Instance;
                
                return (true, new ReferenceTarget() { Instance = instance, PropertyName = prop});
            }

            return (false, null);
        };

        private static InflatedNode LookupName(string name, InflatedNode node)
        {
            var root = FindRoot(node);

            return LookupNameCore(name, root);
        }

        private static InflatedNode LookupNameCore(string name, InflatedNode root)
        {
            if (string.Equals(root.Name, name))
            {
                return root;
            }

            var children = root.GetAllChildren();

            return children.Select(inflatedNode => LookupNameCore(name, inflatedNode)).FirstOrDefault(childLookup => childLookup != null);
        }

        private static InflatedNode FindRoot(InflatedNode node)
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