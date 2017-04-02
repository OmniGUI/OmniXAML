namespace OmniXaml.ReworkPhases
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Rework;

    public class Phase1Builder
    {
        private readonly ISmartInstanceCreator instanceCreator;
        private readonly IStringSourceValueConverter converter;
        private readonly IMemberAssigmentApplier assigmentApplier;

        public Phase1Builder(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter converter, IMemberAssigmentApplier assigmentApplier)
        {
            this.instanceCreator = instanceCreator;
            this.converter = converter;
            this.assigmentApplier = assigmentApplier;
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

            var unassigned = ApplyAssignments(assignments, instance);

            children.AssociateTo(instance);

            return new InflatedNode
            {
                Instance = instance,
                UnresolvedAssignments = new HashSet<InflatedMemberAssignment>(unassigned),
            };
        }

        private IList<InflatedMemberAssignment> ApplyAssignments(IEnumerable<InflatedMemberAssignment> assignments, object instance)
        {
            IList<InflatedMemberAssignment> unassigned = new Collection<InflatedMemberAssignment>();
            foreach (var inflatedMemberAssignment in assignments)
            {
                if (!assigmentApplier.TryApply(inflatedMemberAssignment, instance))
                {
                    unassigned.Add(inflatedMemberAssignment);
                }
            }

            return unassigned;
        }
    }
}