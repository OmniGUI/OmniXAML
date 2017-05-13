namespace OmniXaml.ReworkPhases
{
    using System.Linq;

    public class ObjectBuilderSecondPass
    {
        private readonly IStringSourceValueConverter converter;

        public ObjectBuilderSecondPass(IStringSourceValueConverter converter)
        {
            this.converter = converter;
        }

        public void Fix(InflatedNode inflatedNode)
        {
            LinkChildrenToParent(inflatedNode);

            if (inflatedNode.ConversionFailed)
            {
                var value = converter.TryConvert(inflatedNode.SourceValue, inflatedNode.InstanceType, new ConvertContext() { Node = inflatedNode});                
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