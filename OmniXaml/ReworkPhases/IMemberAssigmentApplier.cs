namespace OmniXaml.ReworkPhases
{
    public interface IMemberAssigmentApplier
    {
        void ExecuteAssignment(InflatedMemberAssignment inflatedAssignment, object instance);
    }
}