namespace OmniXaml
{
    public interface IMemberAssigmentApplier
    {
        void ExecuteAssignment(MemberAssignment inflatedAssignment, object instance);
    }
}