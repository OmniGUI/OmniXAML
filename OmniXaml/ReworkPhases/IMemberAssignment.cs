namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;

    public interface IMemberAssignment
    {
        Member Member { get; }
        ValueCollection Values { get; }
    }
}