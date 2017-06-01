using System;

namespace OmniXaml.Tests
{
    public class FuncAssignmentApplier : IMemberAssigmentApplier
    {
        private Action<NodeAssignment, INodeToObjectBuilder, BuilderContext>  func;

        public FuncAssignmentApplier(Action<NodeAssignment, INodeToObjectBuilder, BuilderContext> func)
        {
            this.func = func;
        }

        public void ExecuteAssignment(NodeAssignment assignment, INodeToObjectBuilder builder, BuilderContext context)
        {
            func(assignment, builder, context);
        }
    }
}