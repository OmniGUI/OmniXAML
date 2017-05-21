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

        public void ExecuteAssignment(MemberAssignment assignment, object instance, INodeToObjectBuilder builder)
        {
            if (assignment.Member.MemberType.IsCollection())
            {
                AssignCollection(assignment, instance);
            }
            else
            {
                AssignSingleValue(assignment, instance, builder);
            }
        }

        private void AssignSingleValue(MemberAssignment assignment, object instance, INodeToObjectBuilder builder)
        {
            var children = assignment.Values.ToList();

            if (children.Count > 1)
            {
                throw new InvalidOperationException($"Cannot assign multiple values to a the property {assignment}");
            }

            var nodeBeingAssigned = children.First();
            
            var value = nodeBeingAssigned.Instance;

            SetMember(instance, assignment.Member, value, nodeBeingAssigned, builder);
        }

        private void SetMember(object parent, Member member, object value, ConstructionNode parentNode, INodeToObjectBuilder builder)
        {
            var mutableUnit = new MutablePipelineUnit(parentNode, value);
            
            pipeline.Handle(parent, member, mutableUnit, builder);
            if (mutableUnit.Handled)
            {
                return;
            }

            member.SetValue(parent, mutableUnit.Value);
        }

        private static void AssignCollection(MemberAssignment assignment, object instance)
        {
            var parent = assignment.Member.GetValue(instance);
            var children = from n in assignment.Values select n.Instance;
            foreach (var child in children)
            {
                Collection.UniversalAdd(parent, child);
            }
        }
    }
}