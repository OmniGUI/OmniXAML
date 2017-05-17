namespace OmniXaml.ReworkPhases
{
    public interface IMemberAssigmentApplier
    {
        void ExecuteAssignment(MemberAssignment inflatedAssignment, object instance);
    }
}