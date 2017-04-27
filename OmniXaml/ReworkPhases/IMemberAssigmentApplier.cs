namespace OmniXaml.ReworkPhases
{
    public interface IMemberAssigmentApplier
    {
        void TryApply(InflatedMemberAssignment inflatedAssignment, object instance);
    }
}