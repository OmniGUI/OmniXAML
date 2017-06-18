using System;
using System.Collections;
using System.Collections.Generic;
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

        public void ExecuteAssignment(NodeAssignment assignment, INodeToObjectBuilder builder, BuilderContext context)
        {
            if (assignment.Assignment.Member.MemberType.IsCollection())
            {
                AssignCollection(assignment);
            }
            else
            {
                AssignSingleValue(assignment, builder, context);
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

        private void AssignCollection(NodeAssignment assignment)
        {
            var firstNode = assignment.Assignment.Values.First();
            var mutableUnit = new MutablePipelineUnit(firstNode, firstNode.Instance);
            pipeline.Handle(assignment.Instance, assignment.Assignment.Member, mutableUnit, null, null);

            if (mutableUnit.Handled)
            {
                return;
            }

            var parent = assignment.Assignment.Member.GetValue(assignment.Instance);

            IEnumerable<object> children;
            var mutableValue = mutableUnit.Value as IEnumerable<object>;

            if (mutableValue != null)
            {
                children = mutableValue;
            }
            else
            {
                children = from n in assignment.Assignment.Values select n.Instance;
            }
            
            foreach (var child in children)
            {
                Collection.UniversalAdd(parent, child);
            }
        }
    }
}