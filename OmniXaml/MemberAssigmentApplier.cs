using System;
using System.Linq;
using Zafiro.Core;

namespace OmniXaml
{
    public class MemberAssigmentApplier : IMemberAssigmentApplier
    {
        private readonly IValuePipeline pipeline;

        public MemberAssigmentApplier(IValuePipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public void ExecuteAssignment(NodeAssignment nodeAssignment, INodeToObjectBuilder builder, BuilderContext context)
        {
            if (nodeAssignment.Assignment.Member.MemberType.IsCollection())
            {
                AssignCollection(nodeAssignment);
            }
            else
            {
                AssignSingleValue(nodeAssignment, builder, context);
            }
        }

        private void AssignSingleValue(NodeAssignment assignment, INodeToObjectBuilder builder, BuilderContext context)
        {
            var children = assignment.Assignment.Values.ToList();

            if (children.Count > 1)
            {
                throw new InvalidOperationException($"Cannot assign multiple values to a the property {assignment}");
            }

            var nodeBeingAssigned = children.First();
            
            var value = nodeBeingAssigned.Instance;

            var assign = new Assignment(assignment.Instance, assignment.Assignment.Member, value);
            SetMember(assign, nodeBeingAssigned, builder, context);
        }

        private void SetMember(Assignment assignment, ConstructionNode parentNode, INodeToObjectBuilder builder, BuilderContext context)
        {
            var mutableUnit = new MutablePipelineUnit(parentNode, assignment.Value);
            
            pipeline.Handle(assignment.Target.Instance, assignment.Member, mutableUnit, builder, context);
            if (mutableUnit.Handled)
            {
                return;
            }

            assignment.Member.SetValue(assignment.Target.Instance, mutableUnit.Value);
        }

        private static void AssignCollection(NodeAssignment assignment)
        {
            var parent = assignment.Assignment.Member.GetValue(assignment.Instance);
            var children = from n in assignment.Assignment.Values select n.Instance;
            foreach (var child in children)
            {
                Collection.UniversalAdd(parent, child);
            }
        }
    }
}