namespace OmniXaml.ReworkPhases
{
    using System.Linq;

    public class Phase2Builder
    {
        private readonly ISmartSourceValueConverter converter;

        public Phase2Builder(ISmartSourceValueConverter converter)
        {
            this.converter = converter;
        }

        public object Resolve(InflatedNode inflatedNode)
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