using System;
using OmniXaml.ReworkPhases;

namespace OmniXaml.Tests
{
    public class FuncAssignmentApplier : IMemberAssigmentApplier
    {
        private Func<InflatedMemberAssignment, object, bool> func;

        public FuncAssignmentApplier(Func<InflatedMemberAssignment, object, bool> func)
        {
            this.func = func;
        }

        public void ExecuteAssignment(InflatedMemberAssignment inflatedAssignment, object instance)
        {
            func(inflatedAssignment, instance);
        }
    }
}