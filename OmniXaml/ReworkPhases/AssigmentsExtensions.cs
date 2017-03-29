namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;
    using System.Linq;
    using Zafiro.Core;

    public static class AssigmentsExtensions
    {
        public static void ApplyTo<T>(this IEnumerable<IMemberAssignment<T>> assignments, object instance) where T : IInstanceHolder
        {
            foreach (var inflatedAssignment in assignments)
            {
                ApplyAssignment(instance, inflatedAssignment);
            }
        }

        private static void ApplyAssignment<T>(object instance, IMemberAssignment<T> inflatedAssignment) where T : IInstanceHolder
        {
            if (inflatedAssignment.Member.MemberType.IsCollection())
            {
                var parent = inflatedAssignment.Member.GetValue(instance);
                Collection.UniversalAdd(parent, from n in inflatedAssignment.Children select n.Instance);
            }
            else
            {
                var value = inflatedAssignment.Children.First().Instance;
                inflatedAssignment.Member.SetValue(instance, value);
            }            
        }
    }
}