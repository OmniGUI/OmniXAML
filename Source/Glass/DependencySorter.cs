namespace Glass
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class DependencySorter
    {
        public static ICollection<T> SortDependencies<T>(this IEnumerable<T> nodes) where T : IDependency<T>
        {
            var set = new HashSet<T>();

            foreach (var node in nodes)
            {
                foreach (var dependency in node.Resolve())
                {
                    set.Add(dependency);
                }                
            }

            return set.ToList();
        }

        public static ICollection<T> Resolve<T>(this T node) where T : IDependency<T>
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