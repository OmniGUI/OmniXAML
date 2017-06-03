namespace OmniXaml
{
    public interface IMemberAssigmentApplier
    {
        void ExecuteAssignment(NodeAssignment assignment, INodeToObjectBuilder builder, BuilderContext context);
    }
}