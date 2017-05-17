using System;
using System.Linq;

namespace OmniXaml.ReworkPhases
{
    public class ObjectBuilderSecondPass
    {
        private readonly IStringSourceValueConverter converter;
        private readonly IMemberAssigmentApplier assigmentApplier;

        public ObjectBuilderSecondPass(IStringSourceValueConverter converter, IMemberAssigmentApplier assigmentApplier)
        {
            this.converter = converter;
            this.assigmentApplier = assigmentApplier;
        }

        public void Fix(InflatedNode inflatedNode)
        {
            //if (inflatedNode.IsPendingCreate)
            //{
            //    Create(inflatedNode);
            //}

            foreach (var assignment in inflatedNode.Assignments)
            {
                foreach (var node in assignment.Values)
                {
                    Fix(node, assignment);
                }
            }

            foreach (var inflatedNodeChild in inflatedNode.Children)
            {
                Fix(inflatedNodeChild);
            }            
        }

        private void Create(InflatedNode inflatedNode)
        {            
        }

        private void Fix(InflatedNode inflatedNode, InflatedMemberAssignment assignment)
        {
            Fix(inflatedNode);

            if (inflatedNode.IsPendingCreate)
            {
                var convertResult = converter.TryConvert(inflatedNode.SourceValue, assignment.Member.MemberType);
                FixNode(assignment, convertResult.Item2);
                ReapplyAssignment(assignment, inflatedNode.Parent.Instance);
            }
        }

        private void ReapplyAssignment(InflatedMemberAssignment assignment, object instance)
        {
            assigmentApplier.ExecuteAssignment(assignment, instance);
        }

        private void FixNode(InflatedMemberAssignment assignment, object instance)
        {
            var nodeToFix = assignment.Values.First();
            nodeToFix.IsPendingCreate = false;
            nodeToFix.Instance = instance;               
        }
    }
}