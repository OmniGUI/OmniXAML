namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;
    using System.Linq;
    using Rework;
    using Zafiro.Core;

    public class Phase1Builder
    {
        private readonly ISmartInstanceCreator instanceCreator;
        private readonly ISmartSourceValueConverter converter;

        public Phase1Builder(ISmartInstanceCreator instanceCreator, ISmartSourceValueConverter converter)
        {
            this.instanceCreator = instanceCreator;
            this.converter = converter;
        }

        public InflatedNode Inflate(ConstructionNode node)
        {
            if (node.SourceValue != null)
            {
                var tryConvert = converter.TryConvert(node.SourceValue, node.ActualInstanceType);
                var isSuccesful = tryConvert.Item1;
                var converted = tryConvert.Item2;

                return new InflatedNode
                {
                    Instance = converted,
                    IsConversionFailed = !isSuccesful,
                    SourceValue = node.SourceValue,
                };
            }

            var children = from n in node.Children select Inflate(n);
            var assignments = from a in node.Assignments
                select new InflatedMemberAssignment
                {
                    Member = a.Member,
                    Children = from c in a.Children select Inflate(c),
                };

            var positionalParameters = from n in node.PositionalParameter select new PositionalParameter(n);
            var creationHints = new CreationHints(new List<NewInjectableMember>(), positionalParameters, new List<object>());

            var instance = instanceCreator.Create(node.ActualInstanceType, creationHints).Instance;
            assignments.ApplyTo(instance);
            children.AssociateTo(instance);

            return new InflatedNode
            {
                Instance = instance,
            };
        }
    }

    public static class InflateNodeExtensions
    {
        public static void AssociateTo(this IEnumerable<InflatedNode> nodes, object parent)
        {
            foreach (var inflatedNode in nodes)
            {
                Collection.UniversalAdd(parent, inflatedNode.Instance);
            }
        }
    }

    public static class AssigmentsExtensions
    {
        public static void ApplyTo(this IEnumerable<InflatedMemberAssignment> assignments, object instance)
        {
            foreach (var inflatedAssignment in assignments)
            {
                ApplyAssignment(instance, inflatedAssignment);
            }
        }

        private static void ApplyAssignment(object instance, InflatedMemberAssignment inflatedAssignment)
        {
            if (inflatedAssignment.Member.MemberType.IsCollection())
            {
                var parent = inflatedAssignment.Member.GetValue(instance);
                Collection.UniversalAdd(parent, from n in inflatedAssignment.Children select n.Instance);
            }
            else
            {
                var value = inflatedAssignment.Children.First().Instance;
                inflatedAssignment.Member.SetValue(instance, value);
            }            
        }
    }
}