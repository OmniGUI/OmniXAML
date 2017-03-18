namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using InlineParsers.Extensions;
    using Zafiro.Core;

    public class NewObjectBuilder : INewObjectBuilder
    {
        private readonly ISmartInstanceCreator instanceCreator;
        private readonly ISmartSourceValueConverter sourceValueConverter;

        public NewObjectBuilder(ISmartInstanceCreator instanceCreator, ISmartSourceValueConverter sourceValueConverter)
        {
            this.instanceCreator = instanceCreator;
            this.sourceValueConverter = sourceValueConverter;
        }

        public object Inflate(ConstructionNode constructionNode)
        {
            var inflateAssignments = InflateAssignments(constructionNode.Assignments);

            var members = GetInjectables(inflateAssignments);

            var positionals =
                from positional in constructionNode.PositionalParameter
                select new PositionalParameter(positional);

            var children = InflateChildren(constructionNode).ToList();

            var creationResult = instanceCreator.Create(constructionNode.InstanceType, new CreationHints(members, positionals, children));

            var unusedMembers = GetMembersNotUsedInConstruction(creationResult.UsedHints.Members, inflateAssignments);

            AssignMembers(creationResult.Instance, unusedMembers, children);

            return creationResult.Instance;
        }

        private IEnumerable<object> InflateChildren(ConstructionNode constructionNode)
        {
            return from c in constructionNode.Children
                let i = Inflate(c)
                select i;
        }

        private IEnumerable<InjectableMember> GetInjectables(IEnumerable<InflatedAssignment> inflatedAssignments)
        {
            var injectableMembers = from child in inflatedAssignments
                select new InjectableMember(child.Instance)
                {
                    Name = child.Member.MemberName,
                    InjectionType = child.Member.MemberType,
                };

            var injectables = injectableMembers;
            return injectables;
        }

        private IEnumerable<InflatedAssignment> GetMembersNotUsedInConstruction(IEnumerable<InjectableMember> creationResultInjectedMembers, IEnumerable<InflatedAssignment> inflatedAssignments)
        {
            var assigned = from injected in creationResultInjectedMembers
                join assignment in inflatedAssignments on injected.Name equals assignment.Member.MemberName

                select assignment;

            return inflatedAssignments.Except(assigned);
        }

        private void AssignMembers(object parent, IEnumerable<InflatedAssignment> inflatedAssignments, List<object> children)
        {
            foreach (var ia in inflatedAssignments)
            {
                ia.Member.SetValue(parent, ia.Instance);
            }

            if (parent.GetType().IsCollection())
            {
                foreach (var child in children)
                {
                    Associate(parent, child);
                }
            }
        }

        private void Associate(object parent, object child)
        {
            Collection.UniversalAdd(parent, child);
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
                    Member = a.Member,
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
                    Member = a.Member,
                };

            return instantiatedAssignments;
        }

        private object GetCompatibleValue(string strValue, Type desiredTargetType)
        {
            return sourceValueConverter.Convert(strValue, desiredTargetType);
        }
    }
}