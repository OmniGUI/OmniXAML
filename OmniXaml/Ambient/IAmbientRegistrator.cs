namespace OmniXaml.Ambient
{
    using System.Collections.Generic;

    public interface IAmbientRegistrator
    {
        void RegisterAssignment(AmbientMemberAssignment assignment);
        IEnumerable<AmbientMemberAssignment> Assigments { get; }
        IEnumerable<object> Instances { get; }
        void RegisterInstance(object instance);
    }
}