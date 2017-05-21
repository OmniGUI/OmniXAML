using System;

namespace OmniXaml.Tests
{
    public class FuncAssignmentApplier : IMemberAssigmentApplier
    {
        private Action<MemberAssignment, object, INodeToObjectBuilder> func;

        public FuncAssignmentApplier(Action<MemberAssignment, object, INodeToObjectBuilder> func)
        {
            this.func = func;
        }

        public void ExecuteAssignment(MemberAssignment assignment, object instance, INodeToObjectBuilder builder)
        {
            func(assignment, instance, builder);
        }
    }
}