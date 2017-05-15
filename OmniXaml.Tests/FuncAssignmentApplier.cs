using System;
using OmniXaml.ReworkPhases;

namespace OmniXaml.Tests
{
    public class FuncAssignmentApplier : IMemberAssigmentApplier
    {
        private Action<InflatedMemberAssignment, object> func;

        public FuncAssignmentApplier(Action<InflatedMemberAssignment, object> func)
        {
            this.func = func;
        }

        public void ExecuteAssignment(InflatedMemberAssignment inflatedAssignment, object instance)
        {
            func(inflatedAssignment, instance);
        }
    }
}