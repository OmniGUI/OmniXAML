namespace Glass.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    public class DependencySorter
    {
        public static ICollection<T> Sort<T>(T node) where T : IDependency<T>
        {
            var resolved = new Collection<T>();
            ResolveDependenciesRecursive(node, resolved, new List<T>());
            return resolved;
        }

        private static void ResolveDependenciesRecursive<T>(T node, ICollection<T> resolved, ICollection<T> seen) where T : IDependency<T>
        {
            Debug.WriteLine(node);
            seen.Add(node);
            foreach (var edge in node.Edges)
            {
                if (!resolved.Contains(edge))
                {
                    if (seen.Contains(edge))
                    {
                        throw new InvalidOperationException($"Circular dependency detected {node} => {edge}");
                    }

                    ResolveDependenciesRecursive(edge, resolved, seen);
                }
            }
            resolved.Add(node);
        }
    }
}