using System;
using OmniXaml.ReworkPhases;

namespace OmniXaml.Tests
{
    public class FuncAssignmentApplier : IMemberAssigmentApplier
    {
        private Action<MemberAssignment, object> func;

        public FuncAssignmentApplier(Action<MemberAssignment, object> func)
        {
            this.func = func;
        }

        public void ExecuteAssignment(MemberAssignment inflatedAssignment, object instance)
        {
            func(inflatedAssignment, instance);
        }
    }
}