namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;
    using System.Linq;
    using Zafiro.Core;

    public static class AssigmentsExtensions
    {
        //public static void ApplyTo<T>(this AssignmentCollection assignments, object instance) where T : IInstanceHolder
        //{
        //    foreach (var inflatedAssignment in assignments)
        //    {
        //        ApplyAssignment(instance, inflatedAssignment);
        //    }
        //}

        //private static void ApplyAssignment<T>(object instance, IMemberAssignment inflatedAssignment) where T : IInstanceHolder
        //{
        //    if (inflatedAssignment.Member.MemberType.IsCollection())
        //    {
        //        var parent = inflatedAssignment.Member.GetValue(instance);
        //        var children = from n in inflatedAssignment.Values select n.Instance;
        //        foreach (var child in children)
        //        {
        //            Collection.UniversalAdd(parent, child);
        //        }                
        //    }
        //    else
        //    {
        //        var value = inflatedAssignment.Values.First().Instance;
        //        inflatedAssignment.Member.SetValue(instance, value);
        //    }            
        //}
    }
}