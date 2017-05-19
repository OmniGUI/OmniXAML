namespace OmniXaml.ReworkPhases
{
    using System;
    using System.Linq;
    using Rework;
    using Zafiro.Core;

    public class MemberAssigmentApplier : IMemberAssigmentApplier
    {
        private readonly IValuePipeline pipeline;

        public MemberAssigmentApplier(IValuePipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public void ExecuteAssignment(MemberAssignment inflatedAssignment, object instance)
        {
            if (inflatedAssignment.Member.MemberType.IsCollection())
            {
                AssignCollection(inflatedAssignment, instance);
            }
            else
            {
                AssignSingleValue(inflatedAssignment, instance);
            }
        }

        private void AssignSingleValue(MemberAssignment assignment, object instance)
        {
            var inflatedAssignmentChildren = assignment.Values.ToList();

            if (inflatedAssignmentChildren.Count > 1)
            {
                throw new InvalidOperationException($"Cannot assign multiple values to a the property {assignment}");
            }

            var nodeBeingAssigned = inflatedAssignmentChildren.First();
            
            var value = nodeBeingAssigned.Instance;

            SetMember(instance, assignment.Member, value, nodeBeingAssigned);
        }

        private void SetMember(object parent, Member member, object value, ConstructionNode parentNode)
        {
            var mutableUnit = new MutablePipelineUnit(parentNode, value);
            pipeline.Handle(parent, member, mutableUnit);
            if (mutableUnit.Handled)
            {
                return;
            }

            member.SetValue(parent, mutableUnit.Value);
        }

        private static void AssignCollection(MemberAssignment inflatedAssignment, object instance)
        {
            var parent = inflatedAssignment.Member.GetValue(instance);
            var children = from n in inflatedAssignment.Values select n.Instance;
            foreach (var child in children)
            {
                Collection.UniversalAdd(parent, child);
            }
        }
    }
}