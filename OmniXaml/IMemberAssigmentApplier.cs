namespace OmniXaml
{
    public interface IMemberAssigmentApplier
    {
        void ExecuteAssignment(MemberAssignment assignment, object instance, INodeToObjectBuilder builder);
    }
}