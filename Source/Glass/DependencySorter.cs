namespace Glass
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class DependencySorter
    {
        public static ICollection<T> Sort<T>(this T node) where T : IDependency<T>
        {
            var resolved = new Collection<T>();
            ResolveDependenciesRecursive(node, resolved, new List<T>());
            return resolved;
        }

        private static void ResolveDependenciesRecursive<T>(T node, ICollection<T> resolved, ICollection<T> notResolved) where T : IDependency<T>
        {
            notResolved.Add(node);
            foreach (var edge in node.Dependencies.Where(edge => !resolved.Contains(edge)))
            {
                if (notResolved.Contains(edge))
                {
                    throw new InvalidOperationException($"Circular dependency detected {node} => {edge}");
                }

                ResolveDependenciesRecursive(edge, resolved, notResolved);
            }

            resolved.Add(node);
            notResolved.Remove(node);
        }
    }
}