namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NewObjectBuilder : INewObjectBuilder
    {
        private readonly ISmartInstanceCreator smartInstanceCreator;
        private readonly ISmartSourceValueConverter sourceValueConverter;

        public NewObjectBuilder(ISmartInstanceCreator smartInstanceCreator, ISmartSourceValueConverter sourceValueConverter)
        {
            this.smartInstanceCreator = smartInstanceCreator;
            this.sourceValueConverter = sourceValueConverter;
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


            var creationResult = smartInstanceCreator.Create(constructionNode.InstanceType, injectableMembers);

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
            return InflateAssignmentsFromChildren(assignments).Concat(AssignmentsFromSourceValues(assignments));
        }

        private IEnumerable<InflatedAssignment> InflateAssignmentsFromChildren(IEnumerable<MemberAssignment> assignments)
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

        private IEnumerable<InflatedAssignment> AssignmentsFromSourceValues(IEnumerable<MemberAssignment> assignments)
        {
            var instantiatedAssignments =
                from a in assignments
                where a.SourceValue != null
                let value = a.SourceValue
                select new InflatedAssignment
                {
                    Instance = GetCompatibleValue(value, a.Member.MemberType),
                    Assignment = a.Member,
                };

            return instantiatedAssignments;
        }

        private object GetCompatibleValue(string strValue, Type desiredTargetType)
        {
            return sourceValueConverter.Convert(strValue, desiredTargetType);
        }
    }
}