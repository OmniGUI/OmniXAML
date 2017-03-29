namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;

    public interface IMemberAssignment<T> where T : IInstanceHolder
    {
        Member Member { get; set; }
        IEnumerable<T> Children { get; set; }
    }
}