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

        public object Fix(InflatedNode inflatedNode)
        {
            var unresolved = from u in inflatedNode.UnresolvedAssignments
                from n in u.Children select n;
            
            foreach (var node in unresolved)
            {
                if (node.SourceValue != null)
                {
                    var tryConvert = converter.TryConvert(node.SourceValue, node.InstanceType);
                    var converted = tryConvert.Item2;
                    node.Instance = converted;
                }
            }

            inflatedNode.UnresolvedAssignments.ApplyTo(inflatedNode.Instance);

            return inflatedNode.Instance;
        }
    }
}