namespace OmniXaml
{
    public interface IMemberAssigmentApplier
    {
        void ExecuteAssignment(NodeAssignment nodeAssignment, INodeToObjectBuilder builder, BuilderContext context);
    }
}