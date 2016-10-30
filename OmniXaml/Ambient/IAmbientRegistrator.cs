namespace OmniXaml.Ambient
{
    using System.Collections.Generic;

    public interface IAmbientRegistrator
    {
        void RegisterAssignment(AmbientPropertyAssignment assignment);
        IEnumerable<AmbientPropertyAssignment> Assigments { get; }
    }
}