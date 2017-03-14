namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;

    public class NewObjectBuilder : INewObjectBuilder
    {
        private readonly IInstanceCook instanceCreator;

        public NewObjectBuilder(IInstanceCook instanceCreator)
        {
            this.instanceCreator = instanceCreator;
        }

        public object Inflate(ConstructionNode constructionNode)
        {
            var inflatedAssignments = InflateAssignments(constructionNode.Assignments);

            var injectableMembers = from child in inflatedAssignments
                select new InjectableMember(child.Instance)
                {
                    Name = child.Assignment.MemberName,
                    InjectionType = child.Assignment.MemberType,
                };


            var creationResult = instanceCreator.Create(constructionNode.InstanceType, injectableMembers);

            var unusedChilden = GetUnusedChildren(creationResult.InjectedMembers, inflatedAssignments);
            ConsumeUnusedAssignments(creationResult.Instance, unusedChilden);

            return creationResult.Instance;
        }

        private IEnumerable<InflatedAssignment> GetUnusedChildren(IEnumerable<InjectableMember> creationResultInjectedMembers, IEnumerable<InflatedAssignment> inflatedAssignments)
        {
            var assigned = from injected in creationResultInjectedMembers
                join assignment in inflatedAssignments on injected.Name equals assignment.Assignment.MemberName

                select assignment;

            return inflatedAssignments.Except(assigned);
        }

        private void ConsumeUnusedAssignments(object parent, IEnumerable<InflatedAssignment> inflatedAssignments)
        {
            foreach (var ia in inflatedAssignments)
            {
                ia.Assignment.SetValue(parent, ia.Instance);
            }
        }

        private IEnumerable<InflatedAssignment> InflateAssignments(IEnumerable<MemberAssignment> assignments)
        {
            return AssignmentsFromComplex(assignments).Concat(AssignmentsFromDirectValues(assignments));
        }

        private IEnumerable<InflatedAssignment> AssignmentsFromComplex(IEnumerable<MemberAssignment> assignments)
        {
            var instantiatedAssignments =
                from a in assignments
                where a.Children.Any()
                from ct in a.Children
                let inflate = Inflate(ct)
                select new InflatedAssignment
                {
                    Instance = inflate,
                    Assignment = a.Member,
                };

            return instantiatedAssignments;
        }

        private IEnumerable<InflatedAssignment> AssignmentsFromDirectValues(IEnumerable<MemberAssignment> assignments)
        {
            var instantiatedAssignments =
                from a in assignments
                where a.SourceValue != null
                let value = a.SourceValue
                select new InflatedAssignment
                {
                    Instance = value,
                    Assignment = a.Member,
                };

            return instantiatedAssignments;
        }
    }
}