using System.Collections.Generic;

namespace OmniXaml.ReworkPhases
{
    using System.Linq;

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
            LinkChildrenToParent(inflatedNode);

            if (inflatedNode.ConversionFailed)
            {
                var convertResult = converter.TryConvert(inflatedNode.SourceValue, inflatedNode.InstanceType, new ConvertContext() { Node = inflatedNode});

                FixAssignment(inflatedNode, convertResult.Item2);                
            }

            var fromAssignments = from u in inflatedNode.Assignments
                             from n in u.Children
                             select n;

            var fromChildren = from o in inflatedNode.Children
                select o;

            foreach (var node in fromChildren.Concat(fromAssignments))
            {
                Fix(node);
            }                      
        }

        private void FixAssignment(InflatedNode inflatedNode, object value)
        {
            var assignmentToFix = inflatedNode.Parent.Assignments.SingleOrDefault(ma => ma.Children.Contains(inflatedNode));

            if (assignmentToFix == null)
            {
                return;
            }

            var childNode = assignmentToFix.Children.First();

            childNode.Instance = value;
            childNode.ConversionFailed = false;

            assigmentApplier.ExecuteAssignment(assignmentToFix, inflatedNode.Parent.Instance);

            FixAssignment(inflatedNode.Parent, inflatedNode.Parent.Instance);
        }

        private void LinkChildrenToParent(InflatedNode parent)
        {
            var fromAssignments = from assignment in parent.Assignments
                             from child in assignment.Children
                             select child;

            var fromChildren = from child in parent.Children select child;

            foreach (var node in fromChildren.Concat(fromAssignments))
            {
                node.Parent = parent;
                LinkChildrenToParent(node);
            }
        }
    }
}